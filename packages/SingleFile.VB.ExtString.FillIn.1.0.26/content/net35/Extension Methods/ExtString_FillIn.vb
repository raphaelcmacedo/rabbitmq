Imports System
Imports System.Diagnostics
Imports System.Globalization
Imports System.Runtime.CompilerServices
Imports System.Text
Imports System.Threading
Imports Microsoft.VisualBasic

<Acknowledgment(Author:="Christoph Hafner", SourceUrl:="https://www.nuget.org/packages/SingleFile.VB.ExtString.FillIn/1.0.26", License:=GetType(MitLicense), Comment:="Leave this acknowledgment 'as-is' and you are legally licensed.")> _
Friend Module ExtString_FillIn

    'Private Fields
    <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
    Private ReadOnly _EmptyArray As Object() = New Object() {}

    'Public Extension Methods

    ''' <summary>Replacement for String.Format(..) that is more tolerant. In detail: It first checks whether the first or last
    ''' entry of the param array is a <see cref="IFormatProvider">format provider</see> and extracts it. In the remaining array null
    ''' entries are replaced through the String "[null]" to better visualize them in the message (if it still is the original array,
    ''' it is first copied to avoid unexpected behavior in case the array is still used for another purpose - of course only if
    ''' there are some null entries). Then the method attempts to call the standard <see cref="String.Format(System.IFormatProvider, String, object[])">String.Format(..)</see>
    ''' function and if successful, returns the result. Otherwise each possible placeholder is attempted to be resolved one by one and
    ''' then added to the result (resolved or not).</summary>
    ''' <param name="format">The String containing placeholders, e.g. <c>"Invoice No {0} of {1:yyyy-MM-dd}"</c>.</param>
    ''' <param name="args">The arguments to be filled into the placeholders, e.g. <c>123456, DateTime.Today, CultureInfo.InvariantCulture</c>.
    ''' You can include a <see cref="IFormatProvider">format provider</see> (here "<see cref="CultureInfo.InvariantCulture">CultureInfo.InvariantCulture</see>") as the first or last argument and it is used to format the numbers,
    ''' dates etc. If no format provider is specified (or somewhere in the middle of the arguments), <see cref="Thread.CurrentCulture">Thread.CurrentThread.CurrentCulture</see>
    ''' is used.</param>
    ''' <returns>The result whereas everything is filled in that is possible to be filled in (no exception is thrown if something cannot be resolved).</returns>
    ''' <exception cref="NullReferenceException">A NullReferenceException is thrown if the format String of which the extension method is invoked is null.</exception>
    ''' <example>
    '''   <para>You are writing a code generator that emits C# code and it contains the follow line of code:</para>
    '''   <c>"public class {0} {".FillIn(myClassName);</c>
    '''   <para>This bears no secrets and results in <c>"public class Xxxx {"</c> as you would expect it, but with standard String.Format(..) all you would get is a <see cref="FormatException" /> (because of the opening "{" that is interpreted as an unterminated placeholder).</para>
    '''   <c>String.Format("public class {0} {", myClassName); //Fails with a FormatException!</c>
    '''   <para>
    ''' With the <c>FillIn</c> extension method you can also provide more arguments than placeholders and do not have to provide subsequent placeholders. This is quite useful if
    ''' you let it up to the translator to use which value, e.g. </para>
    '''   <c>
    ''' //Translation from database if language is "en-US"<br />
    ''' String myMessage = "You are driving at {1} mph.";<br />
    ''' String myResult = myMessage.FillIn(mySpeedInKmh, mySpeedInMph, mySpeedInKn);<br /><br />
    ''' //Translation from database if language is "en-KE"<br />
    ''' String myMessage = "You are driving at {0} km/h.";<br />
    ''' //Translation from database if language is "en-KI"<br />
    ''' String myMessage = "You are driving at {2} kn.";<br />
    ''' String myResult = myMessage.FillIn(mySpeedInKmh, mySpeedInMph, mySpeedInKn);<br /><br /></c>
    ''' Another usage is to avoid exceptions that throw an exception (sounds funny but is quite a common issue). As many exceptions are rarely thrown
    ''' many of them got never tested. If now the number of parameters do not match the placeholders, the original exception is lost and a FormatException
    ''' is thrown, pointing you completely in the wrong direction...
    ''' </example>
    <Extension()> _
    Public Function FillIn(format As String, ParamArray args As Object()) As String
        'Check whether the format is null
        If (format Is Nothing) Then
            Throw New NullReferenceException("format")
        End If
        'Normalize args and extract format provider
        Dim myArgsAndFormat As ArgsAndFormat = ExtractArgsAndFormatProvider(args)
        args = myArgsAndFormat.Arguments
        Dim myFormatProvider As IFormatProvider = myArgsAndFormat.FormatProvider
        'Try to format it natively
        Try
            Return String.Format(myFormatProvider, format, args)
        Catch
        End Try
        'Initialize result
        Dim myResult As New StringBuilder(CType(Math.Truncate(format.Length * 1.5), Int32))
        Dim myPlaceholder As New StringBuilder(10)
        Dim myIsInPlaceholder As Boolean = False
        Dim myIsEscaped As Boolean = False
        For Each myChar As Char In format
            If myIsInPlaceholder Then
                'Append an escaped Char
                If myIsEscaped Then
                    myPlaceholder.Append(myChar)
                    myIsEscaped = False
                    Continue For
                End If
                'Check whether the next Char is escaped
                If myChar = "\"c Then
                    myPlaceholder.Append(myChar)
                    myIsEscaped = True
                    Continue For
                End If
                'Is opening bracket (resets accumulated content)
                If myChar = "{"c Then
                    myResult.Append(myPlaceholder.ToString())
                    myPlaceholder = New StringBuilder(myChar)
                    Continue For
                End If
                'Is closing bracket (parse content, reset buffer)
                If myChar = "}"c Then
                    myPlaceholder.Append(myChar)
                    Dim myToken As String = TryReplacePlaceholder(myPlaceholder.ToString(), args, myFormatProvider)
                    myResult.Append(myToken)
                    myPlaceholder = New StringBuilder()
                    myIsInPlaceholder = False
                    Continue For
                End If
                'Is normal placeholder content Char
                myPlaceholder.Append(myChar)
            Else
                'Begin of placeholder
                If myChar = "{"c Then
                    myPlaceholder.Append(myChar)
                    myIsInPlaceholder = True
                    Continue For
                End If
                'Append constant Char
                myResult.Append(myChar)
            End If
        Next
        'Append last buffer
        If myIsInPlaceholder Then
            myResult.Append(myPlaceholder.ToString())
        End If
        'Return the result
        Return myResult.ToString()
    End Function

    'Private Methods

    Private Function ExtractArgsAndFormatProvider(args As Object()) As ArgsAndFormat
        'Check null
        If args Is Nothing Then
            Return New ArgsAndFormat(_EmptyArray, Thread.CurrentThread.CurrentCulture)
        End If
        'Correct incorrect C# bindings
        args = UnwrapIncorrectCSharpBindings(args)
        Dim myLength As Int32 = args.Length
        'Check if empty (after unwrapping!)
        If args.Length = 0 Then
            Return New ArgsAndFormat(_EmptyArray, Thread.CurrentThread.CurrentCulture)
        End If
        'Check whether the first arg is the format provider
        Dim myFormatProvider As IFormatProvider
        Dim myFirstArg As Object = args(0)
        myFormatProvider = TryCast(myFirstArg, IFormatProvider)
        If (myFormatProvider IsNot Nothing) Then
            'Check whether there is only a format provider
            If args.Length = 1 Then
                Return New ArgsAndFormat(_EmptyArray, myFormatProvider)
            End If
            'Otherwise copy array
            Dim myNewArray As Object() = New Object(myLength - 2) {}
            Array.Copy(args, 1, myNewArray, 0, myNewArray.Length)
            ReplaceNull(myNewArray)
            myNewArray = UnwrapIncorrectCSharpBindings(myNewArray)
            Return New ArgsAndFormat(myNewArray, myFormatProvider)
        End If
        'Check whether the last arg is the format provider
        If (args.Length > 1) Then
            Dim myLastArg As Object = args(args.Length - 1)
            myFormatProvider = TryCast(myLastArg, IFormatProvider)
            If (myFormatProvider IsNot Nothing) Then
                'Copy the array
                Dim myNewArray As Object() = New Object(myLength - 2) {}
                Array.Copy(args, 0, myNewArray, 0, myNewArray.Length)
                ReplaceNull(myNewArray)
                myNewArray = UnwrapIncorrectCSharpBindings(myNewArray)
                Return New ArgsAndFormat(myNewArray, myFormatProvider)
            End If
        End If
        'Otherwise replace null values (on a copy of the array!)
        args = ReplaceNullAndReturnCopy(args)
        Return New ArgsAndFormat(args, Thread.CurrentThread.CurrentCulture)
    End Function

    Private Sub ReplaceNull(args As Object())
        If (args Is Nothing) OrElse (args.Length = 0) Then
            Return
        End If
        For i As Int32 = 0 To args.Length - 1
            Dim myArg As Object = args(i)
            If (myArg Is Nothing) Then
                'Replace null through "[null]"
                args(i) = "[null]"
            Else
                'Replace ASCII-NUL values through "[\0]"
                Dim myStringArg As String = TryCast(myArg, String)
                If (myStringArg IsNot Nothing) Then
                    If HasNULChar(myStringArg) Then
                        Dim myResult As New StringBuilder(myStringArg.Length + 3) 'Usually there is no more than 1 NUL-char
                        For Each myChar As Char In myStringArg
                            If (myChar = ControlChars.NullChar) Then
                                myResult.Append("[\0]")
                            Else
                                myResult.Append(myChar)
                            End If
                        Next
                        args(i) = myResult.ToString()
                    End If
                End If
            End If
        Next
    End Sub

    Private Function HasNULChar(myArg As String) As Boolean
        For Each myChar As Char In myArg
            If (myChar = ControlChars.NullChar) Then Return True
        Next
        Return False
    End Function

    Private Function UnwrapIncorrectCSharpBindings(args As Object()) As Object()
        If (args Is Nothing) Then Return _EmptyArray
        If (args.Length <> 1) Then Return args
        Dim myResult As Object() = TryCast(args(0), Object())
        If (myResult Is Nothing) Then Return args
        Return myResult
    End Function

    Private Function ReplaceNullAndReturnCopy(args As Object()) As Object()
        'By default, assign same instance
        Dim myResult As Object() = args
        'Loop through args. As soon as an entry is null, copy array and continue looping at same array position but performing another action
        Dim i As Int32 = 0
        While (i < myResult.Length)
            If myResult(i) Is Nothing Then
                'Copy the given array and re-assign it to myResult
                myResult = New Object(myResult.Length - 1) {}
                Array.Copy(args, myResult, args.Length)
                'Replace the NULL value at the current index
                myResult(i) = "[null]"
                'Increase i
                i += 1
                'Continue looping with code to replace only
                GoTo ReplaceNull
            End If
            i += 1
        End While
        'Return original array (unchanged)
        Return myResult
ReplaceNull:
        'Continue replacing null values on copied array
        While i < myResult.Length
            If myResult(i) Is Nothing Then
                myResult(i) = "[null]"
            End If
            i += 1
        End While
        'Return copied array (null values replaced)
        Return myResult
    End Function

    Private Function TryReplacePlaceholder(singlePlaceholder As String, args As Object(), formatProvider As IFormatProvider) As String
        'Check length
        If (singlePlaceholder Is Nothing) OrElse (singlePlaceholder.Length < 3) Then
            Return singlePlaceholder
        End If
        'Hint: Smallest placeholder is "{0}";
        If (args Is Nothing) OrElse (args.Length = 0) Then
            Return singlePlaceholder
        End If
        'Check whether starts and ends with { }
        If (singlePlaceholder(0) <> "{"c) OrElse (singlePlaceholder(singlePlaceholder.Length - 1) <> "}"c) Then
            Return singlePlaceholder
        End If
        'Split format String at the ":" sign
        Dim myArrayIndexAsString As String = Nothing
        '"0" if the format is "{0,5:###.0}"
        Dim myColumnLengthString As String = Nothing
        '",5" if the format is "{0,5:###.0}"
        Dim myFormatting As String = Nothing
        '"###.0" if the format is "{0,5:###.0}"
        Dim myColon As Int32 = singlePlaceholder.IndexOf(":"c)
        If myColon < 0 Then
            myArrayIndexAsString = singlePlaceholder.Substring(1, singlePlaceholder.Length - 2).Trim()
        Else
            myArrayIndexAsString = singlePlaceholder.Substring(1, myColon - 1).Trim()
            myFormatting = singlePlaceholder.Substring(myColon + 1, singlePlaceholder.Length - myColon - 2).Trim()
        End If
        'The array index may also have a comma and some formatting
        Dim myArrayIndexParser As New ArrayIndexParser(myArrayIndexAsString)
        Dim myArrayIndex As Int32 = myArrayIndexParser.ArrayIndex
        If myArrayIndex < 0 Then
            Return singlePlaceholder
        End If
        If myArrayIndex >= args.Length Then
            Return singlePlaceholder
        End If
        myColumnLengthString = myArrayIndexParser.RemainingString
        'Get the according object
        Dim myArg As Object = args(myArrayIndex)
        Try
            Dim myFormat As String = "{0" & (If(myColumnLengthString, "")) & (If((myColon < 0), "", ":")) & (If(myFormatting, "")) & "}"c
            Return String.Format(formatProvider, myFormat, myArg)
        Catch
            Return singlePlaceholder
        End Try
    End Function


    '*****************************************************************************************
    ' INNER CLASS: ArrayIndexParser
    '*****************************************************************************************

    Private Class ArrayIndexParser

        'Constructors

        Public Sub New(arrayIndexString As String)
            arrayIndexString = arrayIndexString.Trim()
            Dim myRemindingString As String = Nothing
            Dim myNumberString As New StringBuilder(1)
            'usually there are hardly more than 9 placeholders...
            For i As Int32 = 0 To arrayIndexString.Length - 1
                Dim myChar As Char = arrayIndexString(i)
                If "0123456789".Contains(myChar) Then
                    myNumberString.Append(myChar)
                Else
                    myRemindingString = arrayIndexString.Substring(i)
                    Exit For
                End If
            Next
            If (myNumberString.Length = 0) OrElse (myNumberString.Length > 9) Then
                'We cheat a bit, Int32.MaxValue would be 10 digits, but not many people are going to add a billion placeholders...
                ArrayIndex = -1
                Return
            End If
            ArrayIndex = Int32.Parse(myNumberString.ToString(), CultureInfo.InvariantCulture)
            RemainingString = If(myRemindingString, "")
        End Sub

        'Public Fields
        Public ReadOnly ArrayIndex As Int32 = -1
        Public ReadOnly RemainingString As String = ""

    End Class


    '*****************************************************************************************
    ' INNER CLASS: ArgsAndFormat
    '*****************************************************************************************

    Private Class ArgsAndFormat

        'Constructors

        Public Sub New(args As Object(), format As IFormatProvider)
            Arguments = args
            FormatProvider = format
        End Sub

        'Public Fields

        Public Arguments As Object()
        Public FormatProvider As IFormatProvider

    End Class

End Module

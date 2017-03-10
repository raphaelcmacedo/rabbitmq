Imports System 'mscorlib.dll
Imports System.Runtime.CompilerServices 'System.Core.dll
Imports System.Numerics 'System.Numerics.dll
Imports System.Collections.Generic 'mscorlib.dll
Imports System.Diagnostics 'mscorlib.dll

<Acknowledgment(Author:="Christoph Hafner", SourceUrl:="https://www.nuget.org/packages/SingleFile.VB.ExtBigInteger.ToByteArray/1.0.25", License:=GetType(MitLicense), Comment:="Leave this acknowledgment 'as-is' and you are legally licensed.")> _
Friend Module ExtBigInteger_ToByteArray

    'Private Fields
    <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
    Private ReadOnly _BI0 As BigInteger = 0
    <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
    Private ReadOnly _BI256 As BigInteger = 256

    'Public Methods

    ''' <summary>Converts the (positive) BigInteger value to a byte array, either with big or
    ''' little endianness. The endianness describes whether the highest digits (=big endianness) or 
    ''' the lowest ones (=little endianness) come first in array order. e.g. the value 795192 
    ''' ((12 * (256 ^ 2)) + (34 * 256) + 56) is written as [12,34,56] using big endianness and 
    ''' [56,34,12] using little endianness.</summary>
    ''' <param name="value">The value to be converted into a byte array (it must be positive or zero).</param>
    ''' <param name="toBigEndian">If set to <c>true</c>, the byte order of the result is using big endianness; otherwise little endianness is used.</param>
    ''' <returns>The value as array of bytes.</returns>
    ''' <exception cref="System.ArgumentOutOfRangeException">An ArgumentOutOfRangeException is thrown if the value is negative.</exception>
    <Extension()> _
    Public Function ToByteArray(value As BigInteger, toBigEndian As Boolean) As Byte()
        'Check the given value
        If (value.Sign < 0) Then
            Throw New ArgumentOutOfRangeException("value", "A positive value or zero expected! The given value was: {0}".FillIn(value))
        End If
        'Return result
        Dim myResult As Byte() = PrivateToByteArray(value, 1, toBigEndian)
        Return myResult
    End Function

    ''' <summary>Converts the (positive) BigInteger value to a byte array, either with big or
    ''' little endianness. The endianness describes whether the highest digits (=big endianness) or
    ''' the lowest ones (=little endianness) come first in array order. e.g. the value 795192
    ''' ((12 * (256 ^ 2)) + (34 * 256) + 56) is written as [12,34,56] using big endianness and
    ''' [56,34,12] using little endianness.</summary>
    ''' <param name="value">The value to be converted into a byte array (it must be positive or zero).</param>
    ''' <param name="minArraySize">Minimum size of the array (additional NUL bytes are added if needed).</param>
    ''' <param name="toBigEndian">If set to <c>true</c>, the byte order of the result is using big endianness; otherwise little endianness is used.</param>
    ''' <returns>The value as array of bytes.</returns>
    ''' <exception cref="System.ArgumentOutOfRangeException">An ArgumentOutOfRangeException is thrown if the value is negative.</exception>
    <Extension()> _
    Public Function ToByteArray(value As BigInteger, minArraySize As Int32, toBigEndian As Boolean) As Byte()
        'Check the given value
        If (value.Sign < 0) Then
            Throw New ArgumentOutOfRangeException("value", "A positive value or zero expected! The given value was: {0}".FillIn(value))
        End If
        If (minArraySize < 1) Then minArraySize = 1
        'Return result
        Dim myResult As Byte() = PrivateToByteArray(value, minArraySize, toBigEndian)
        Return myResult
    End Function

    'Private Fields

    Private Function PrivateToByteArray(value As BigInteger, minArraySize As Int32, toBigEndian As Boolean) As Byte()
        'Initialize the result
        Dim myCapacity As Int32 = Math.Max(10, minArraySize)
        Dim myList As New List(Of Byte)(myCapacity)
        While (value > _BI0)
            Dim myByte As BigInteger = (value Mod _BI256)
            myList.Add(CType(myByte, Byte))
            value -= myByte
            value /= _BI256
        End While
        'Fill in additional zeros
        If (minArraySize > myList.Count) Then
            myList.AddRange(New Byte(minArraySize - myList.Count - 1) {})
        End If
        'Convert to array
        Dim myResult As Byte() = myList.ToArray()
        'Consider endianness
        If (toBigEndian) Then
            Array.Reverse(myResult)
        End If
        'Return the result
        Return myResult
    End Function

End Module

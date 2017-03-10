#If DEBUG Then
'----------------------------------------------------------------------------------------
Imports System
Imports System.Threading
Imports System.Globalization
Imports Microsoft.VisualStudio.TestTools.UnitTesting 'Microsoft.VisualStudio.QualityTools.UnitTestFramework.dll (with "Copy Local = False")

<TestClass()> _
<Acknowledgment(Author:="Christoph Hafner", SourceUrl:="https://www.nuget.org/packages/SingleFile.VB.ExtString.FillIn/1.0.26", License:=GetType(MitLicense), Comment:="Leave this acknowledgment 'as-is' and you are legally licensed.")> _
Public Class Test_ExtString_FillIn

    <TestMethod()> _
    <TestCategory("Extension Methods")> _
    Public Sub ExtString_FillIn()
        'Initialize default format provider to de-CH, language to en-US
        Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("de-CH")
        Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture
        'Test only exception that is expected
        Dim myFormat As String = Nothing
        Dim myMessage As String = Nothing
        Try
            myFormat.FillIn("a", "b", "c")
            Assert.Fail("A NullReferenceException should have been thrown!")
        Catch ex As Exception
            Assert.IsInstanceOfType(ex, GetType(NullReferenceException))
        End Try
        'No other exceptions should be thrown...
        Try
            'Test "{0:yyyy-MM-dd}: {3,10:0.00} { 4 : 0 } {5,10} {6:dd.MM.yyyy}"

            'Test a simple date format
            myFormat = "Today is {0:yyyy-MM-dd}."
            myMessage = myFormat.FillIn(New DateTime(2015, 12, 31))
            Assert.AreEqual("Today is 2015-12-31.", myMessage)
            'Test null value replacement
            myFormat = "The values are {0}, {1} and {2}."
            myMessage = myFormat.FillIn(Nothing, Nothing, 33)
            Assert.AreEqual("The values are [null], [null] and 33.", myMessage)
            'Test too many parameters
            myFormat = "Today is {1:yyyy-MM-dd}."
            myMessage = myFormat.FillIn(New DateTime(2015, 12, 31), New DateTime(2015, 11, 30), New DateTime(2015, 10, 31))
            Assert.AreEqual("Today is 2015-11-30.", myMessage)
            'Test too many placeholders
            myFormat = "The values are {0:0.###}, {1} and {2}."
            myMessage = myFormat.FillIn(18.45, True)
            Assert.AreEqual("The values are 18.45, True and {2}.", myMessage)
            'Test incorrect format
            myFormat = "Today is {0:yyyy-MM-dd}."
            myMessage = myFormat.FillIn(Guid.NewGuid())
            Assert.AreEqual("Today is {0:yyyy-MM-dd}.", myMessage)
            'Test confusing format
            myFormat = "public class {0} { //partial class, inherits {1}"
            myMessage = myFormat.FillIn("myClass", "myParentClass")
            Assert.AreEqual("public class myClass { //partial class, inherits myParentClass", myMessage)
            'Test format with leading spaces, test invert order
            myFormat = "{1,20}|{0,12:000,000.00}"
            myMessage = myFormat.FillIn(123.45, "Total amount USD")
            Assert.AreEqual("    Total amount USD|  000'123.45", myMessage)
            'Test multiple occurrences
            myFormat = "Raw: {0:0.########}, Rounded: {0:0}, Another number: {1}, Again the first one with 2 digits: {0:0.##}"
            myMessage = myFormat.FillIn(123.456789, 987.65)
            Assert.AreEqual("Raw: 123.456789, Rounded: 123, Another number: 987.65, Again the first one with 2 digits: 123.46", myMessage)
            'The format provider
            Dim myProvider1 As IFormatProvider = CultureInfo.GetCultureInfo("en-US")
            Dim myProvider2 As IFormatProvider = CultureInfo.GetCultureInfo("de-DE")
            Dim myProvider3 As IFormatProvider = CultureInfo.GetCultureInfo("de-CH")
            myFormat = "Number: {0:N}"
            myMessage = myFormat.FillIn(myProvider1, 12345.6789)
            Assert.AreEqual("Number: 12,345.68", myMessage)
            'en-US
            myMessage = myFormat.FillIn(12345.6789, myProvider2)
            Assert.AreEqual("Number: 12.345,68", myMessage)
            'de-DE
            myMessage = myFormat.FillIn(myProvider3, 12345.6789, myProvider1)
            'First should be taken, second is a simple argument
            'de-CH
            Assert.AreEqual("Number: 12'345.68", myMessage)
        Catch ex As Exception When (Not TypeOf ex Is AssertFailedException) And (Not TypeOf ex Is AssertInconclusiveException) 'VB 10 support
            Assert.Fail("An exception of type " & ex.GetType().FullName & " occurred, but no exception should have been thrown!")
        End Try
    End Sub

End Class
'----------------------------------------------------------------------------------------
#End If

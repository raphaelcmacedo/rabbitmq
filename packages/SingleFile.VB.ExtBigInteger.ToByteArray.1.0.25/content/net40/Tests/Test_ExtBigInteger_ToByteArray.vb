#If DEBUG Then
'----------------------------------------------------------------------------------------
Imports System
Imports System.Numerics 'System.Numerics.dll
Imports Microsoft.VisualStudio.TestTools.UnitTesting 'Microsoft.VisualStudio.QualityTools.UnitTestFramework.dll (with "Copy Local = False")

<TestClass()>
<Acknowledgment(Author:="Christoph Hafner", SourceUrl:="https://www.nuget.org/packages/SingleFile.VB.ExtBigInteger.ToByteArray/1.0.25", License:=GetType(MitLicense), Comment:="Leave this acknowledgment 'as-is' and you are legally licensed.")>
Public Class Test_ExtBigInteger_ToByteArray

    <TestMethod()>
    <TestCategory("Extension Methods")>
    Public Sub ExtBigInteger_ToByteArray()
        'Declare variables
        Dim myInput As BigInteger = 0
        Dim myExpected As Byte() = Nothing
        Dim myResult As Byte() = Nothing
        'Negative number (little endian)
        Try
            myInput = -1000
            myResult = myInput.ToByteArray(False)
            Assert.Fail("An exception of type {0} expected!", GetType(ArgumentOutOfRangeException).FullName)
        Catch ex As Exception When (Not TypeOf ex Is AssertFailedException) And (Not TypeOf ex Is AssertInconclusiveException) 'VB 10 support
            Assert.IsInstanceOfType(ex, GetType(ArgumentOutOfRangeException), "An exception of type {0} expected!", GetType(ArgumentOutOfRangeException).FullName)
        End Try
        'Negative number (big endian)
        Try
            myInput = -1000
            myResult = myInput.ToByteArray(True)
            Assert.Fail("An exception of type {0} expected!", GetType(ArgumentOutOfRangeException).FullName)
        Catch ex As Exception When (Not TypeOf ex Is AssertFailedException) And (Not TypeOf ex Is AssertInconclusiveException) 'VB 10 support
            Assert.IsInstanceOfType(ex, GetType(ArgumentOutOfRangeException), "An exception of type {0} expected!", GetType(ArgumentOutOfRangeException).FullName)
        End Try
        'Negative number (little endian, min length)
        Try
            myInput = -1000
            myResult = myInput.ToByteArray(10, False)
            Assert.Fail("An exception of type {0} expected!", GetType(ArgumentOutOfRangeException).FullName)
        Catch ex As Exception When (Not TypeOf ex Is AssertFailedException) And (Not TypeOf ex Is AssertInconclusiveException) 'VB 10 support
            Assert.IsInstanceOfType(ex, GetType(ArgumentOutOfRangeException), "An exception of type {0} expected!", GetType(ArgumentOutOfRangeException).FullName)
        End Try
        'Negative number (big endian, min length)
        Try
            myInput = -1000
            myResult = myInput.ToByteArray(10, True)
            Assert.Fail("An exception of type {0} expected!", GetType(ArgumentOutOfRangeException).FullName)
        Catch ex As Exception When (Not TypeOf ex Is AssertFailedException) And (Not TypeOf ex Is AssertInconclusiveException) 'VB 10 support
            Assert.IsInstanceOfType(ex, GetType(ArgumentOutOfRangeException), "An exception of type {0} expected!", GetType(ArgumentOutOfRangeException).FullName)
        End Try
        'Zero (little endian)
        Try
            myInput = 0
            myResult = myInput.ToByteArray(False)
        Catch ex As Exception
            Assert.Fail("No exception should have been thrown!")
        End Try
        myExpected = New Byte() {0}
        Assert.IsTrue(ArrayEquals(myResult, myExpected), "A byte array with a single 00 expected!")
        'Zero (big endian)
        Try
            myInput = 0
            myResult = myInput.ToByteArray(True)
        Catch ex As Exception
            Assert.Fail("No exception should have been thrown!")
        End Try
        myExpected = New Byte() {0}
        Assert.IsTrue(ArrayEquals(myResult, myExpected), "A byte array with a single 00 expected!")
        'Zero (little endian, min length)
        Try
            myInput = 0
            myResult = myInput.ToByteArray(4, False)
        Catch ex As Exception
            Assert.Fail("No exception should have been thrown!")
        End Try
        myExpected = New Byte() {0, 0, 0, 0}
        Assert.IsTrue(ArrayEquals(myResult, myExpected), "A byte array with [00, 00, 00, 00] expected!")
        'Zero (big endian, min length)
        Try
            myInput = 0
            myResult = myInput.ToByteArray(4, True)
        Catch ex As Exception
            Assert.Fail("No exception should have been thrown!")
        End Try
        myExpected = New Byte() {0, 0, 0, 0}
        Assert.IsTrue(ArrayEquals(myResult, myExpected), "A byte array with [00, 00, 00, 00] expected!")
        'Positive number (little endian)
        Try
            myInput = 795192
            myResult = myInput.ToByteArray(False)
        Catch ex As Exception
            Assert.Fail("No exception should have been thrown!")
        End Try
        myExpected = New Byte() {56, 34, 12}
        Assert.IsTrue(ArrayEquals(myResult, myExpected), "A byte array with [56, 34, 12] expected!")
        'Positive number (big endian)
        Try
            myInput = 795192
            myResult = myInput.ToByteArray(True)
        Catch ex As Exception
            Assert.Fail("No exception should have been thrown!")
        End Try
        myExpected = New Byte() {12, 34, 56}
        Assert.IsTrue(ArrayEquals(myResult, myExpected), "A byte array with [12, 34, 56] expected!")
        'Positive number (little endian, min length)
        Try
            myInput = 795192
            myResult = myInput.ToByteArray(5, False)
        Catch ex As Exception
            Assert.Fail("No exception should have been thrown!")
        End Try
        myExpected = New Byte() {56, 34, 12, 00, 00}
        Assert.IsTrue(ArrayEquals(myResult, myExpected), "A byte array with [56, 34, 12, 00, 00] expected!")
        'Positive number (big endian, min length)
        Try
            myInput = 795192
            myResult = myInput.ToByteArray(5, True)
        Catch ex As Exception
            Assert.Fail("No exception should have been thrown!")
        End Try
        myExpected = New Byte() {00, 00, 12, 34, 56}
        Assert.IsTrue(ArrayEquals(myResult, myExpected), "A byte array with [00, 00, 12, 34, 56] expected!")
        'Positive number (little endian, min length less than real length)
        Try
            myInput = 795192
            myResult = myInput.ToByteArray(2, False)
        Catch ex As Exception
            Assert.Fail("No exception should have been thrown!")
        End Try
        myExpected = New Byte() {56, 34, 12}
        Assert.IsTrue(ArrayEquals(myResult, myExpected), "A byte array with [56, 34, 12] expected!")
        'Positive number (big endian, min length less than real length)
        Try
            myInput = 795192
            myResult = myInput.ToByteArray(2, True)
        Catch ex As Exception
            Assert.Fail("No exception should have been thrown!")
        End Try
        myExpected = New Byte() {12, 34, 56}
        Assert.IsTrue(ArrayEquals(myResult, myExpected), "A byte array with [12, 34, 56] expected!")
    End Sub

    'Private Methods

    Private Shared Function ArrayEquals(result As Byte(), expected As Byte()) As Boolean
        'Handle null
        If (result Is Nothing) Then
            If (expected Is Nothing) Then Return True
            Return False
        End If
        If (expected Is Nothing) Then Return False
        'Compare length
        If (result.Length <> expected.Length) Then Return False
        'Compare content
        For i As Int32 = 0 To result.Length - 1
            Dim x As Byte = result(i)
            Dim y As Byte = expected(i)
            If (x <> y) Then Return False
        Next
        'Otherwise return true
        Return True
    End Function

End Class
'----------------------------------------------------------------------------------------
#End If

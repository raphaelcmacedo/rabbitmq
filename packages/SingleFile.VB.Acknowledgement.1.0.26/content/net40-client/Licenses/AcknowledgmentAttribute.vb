Imports System
Imports System.Diagnostics

''' <summary>The class AcknowledgmentAttribute is an attribute that may be applied anywhere in code to give 
''' credit to the contributor of that code and to specify what license applies for that piece of code.</summary>
<AttributeUsage(AttributeTargets.Assembly Or AttributeTargets.Class Or AttributeTargets.Enum Or AttributeTargets.Interface Or AttributeTargets.Struct Or AttributeTargets.Module Or AttributeTargets.Method Or AttributeTargets.Field Or AttributeTargets.Property Or AttributeTargets.Event, AllowMultiple:=True, Inherited:=False)> _
Friend NotInheritable Class AcknowledgmentAttribute
    Inherits Attribute

    'Private Fields
    <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
    Private _Author As String
    <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
    Private _SourceUrl As String
    <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
    Private _License As Type
    <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
    Private _Acknowledgment As String
    <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
    Private _Comment As String

    'Constructors

    ''' <summary>Empty constructor that allows to specify the information through "Property:=Value" syntax.</summary>
    Public Sub New()
    End Sub

    ''' <summary>Defines a copyright, the source where the code comes from, the license that applies, as well as textual acknowledgments and comments.</summary>
    ''' <param name="author">Owner of the code, usually the person or company that wrote it.</param>
    ''' <param name="sourceUrl">Where was this code downloaded from?</param>
    ''' <param name="license">The type that holds the license information, that should be a subclass of <see cref="LicenseInfo" />.</param>
    ''' <param name="acknowledgment">A "thank you" statement to the writer of the code.</param>
    ''' <param name="comment">Some additional comments that should be included in the assembly.</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal author As String, ByVal sourceUrl As String, ByVal license As Type, ByVal acknowledgment As String, ByVal comment As String)
        Me.Author = author
        Me.SourceUrl = sourceUrl
        Me.License = license
        Me.Acknowledgment = acknowledgment
        Me.Comment = comment
    End Sub

    'Public Properties

    ''' <summary>Owner of the code, usually the person or company that wrote it.</summary>
    Public Property Author() As String
        Get
            Return _Author
        End Get
        Set(ByVal value As String)
            _Author = value
        End Set
    End Property

    ''' <summary>The place where this code was downloaded from.</summary>
    Public Property SourceUrl() As String
        Get
            Return _SourceUrl
        End Get
        Set(ByVal value As String)
            _SourceUrl = value
        End Set
    End Property

    ''' <summary>The type of the class (usually containing a singleton-property) fully describing the license. The type should be a subclass of <see cref="LicenseInfo" />.</summary>
    Public Property License() As Type
        Get
            Return _License
        End Get
        Set(ByVal value As Type)
            _License = value
        End Set
    End Property

    ''' <summary>Some "Thank you" words addressed to the writer of the code.</summary>
    Public Property Acknowledgment() As String
        Get
            Return _Acknowledgment
        End Get
        Set(ByVal value As String)
            _Acknowledgment = value
        End Set
    End Property

    ''' <summary>"Additional comments that should be included in the assembly.</summary>
    Public Property Comment() As String
        Get
            Return _Comment
        End Get
        Set(ByVal value As String)
            _Comment = value
        End Set
    End Property

End Class

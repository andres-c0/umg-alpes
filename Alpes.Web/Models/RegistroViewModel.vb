Option Strict On
Option Explicit On

Namespace Models
    Public Class RegistroViewModel
        Public Property Username As String
        Public Property Email As String
        Public Property Password As String
        Public Property ConfirmPassword As String

        Public Property TipoDocumento As String
        Public Property NumDocumento As String
        Public Property Nit As String
        Public Property Nombres As String
        Public Property Apellidos As String
        Public Property TelResidencia As String
        Public Property TelCelular As String
        Public Property Direccion As String
        Public Property Ciudad As String
        Public Property Departamento As String
        Public Property Pais As String
        Public Property Profesion As String

        Public Property ReturnUrl As String

        Public ReadOnly Property TelefonoPrincipal As String
            Get
                If Not String.IsNullOrWhiteSpace(TelCelular) Then
                    Return TelCelular.Trim()
                End If

                If Not String.IsNullOrWhiteSpace(TelResidencia) Then
                    Return TelResidencia.Trim()
                End If

                Return String.Empty
            End Get
        End Property
    End Class
End Namespace
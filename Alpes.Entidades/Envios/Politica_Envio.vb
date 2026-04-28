Option Strict On
Option Explicit On

Namespace Envios
    Public Class Politica_Envio
        Public Property PoliticaEnvioId As Integer
        Public Property Titulo As String
        Public Property Descripcion As String
        Public Property VigenciaInicio As DateTime?
        Public Property VigenciaFin As DateTime?
        Public Property CreatedAt As DateTime?
        Public Property UpdatedAt As DateTime?
        Public Property Estado As String
    End Class
End Namespace
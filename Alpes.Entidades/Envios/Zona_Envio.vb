Option Strict On
Option Explicit On

Namespace Envios
    Public Class Zona_Envio
        Public Property ZonaEnvioId As Integer
        Public Property Nombre As String
        Public Property Pais As String
        Public Property Departamento As String
        Public Property Ciudad As String
        Public Property CreatedAt As DateTime?
        Public Property UpdatedAt As DateTime?
        Public Property Estado As String
    End Class
End Namespace
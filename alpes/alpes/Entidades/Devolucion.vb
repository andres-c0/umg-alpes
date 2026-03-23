Namespace Entidades

    Public Class Devolucion

        Public Property DevolucionId As Integer
        Public Property OrdenVentaId As Integer
        Public Property CliId As Integer
        Public Property Motivo As String
        Public Property EstadoDevolucion As String
        Public Property SolicitudAt As DateTime
        Public Property ResolucionAt As DateTime?
        Public Property Estado As String

    End Class

End Namespace

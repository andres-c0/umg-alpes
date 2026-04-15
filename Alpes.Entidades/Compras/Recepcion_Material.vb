Option Strict On
Option Explicit On

Namespace Compras
    Public Class Recepcion_Material
        Public Property RecepcionMaterialId As Integer
        Public Property OrdenCompraId As Integer
        Public Property NumOc As String
        Public Property FechaRecepcion As DateTime
        Public Property EmpIdRecibe As Integer?
        Public Property EmpleadoRecibe As String
        Public Property Observaciones As String
        Public Property CreatedAt As DateTime?
        Public Property UpdatedAt As DateTime?
        Public Property Estado As String
    End Class
End Namespace
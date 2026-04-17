Option Strict On
Option Explicit On

Namespace Models
    Public Class ProductoListadoViewModel
        Public Property ProductoId As Integer
        Public Property Referencia As String
        Public Property Nombre As String
        Public Property Descripcion As String
        Public Property Tipo As String
        Public Property Material As String
        Public Property AltoCm As Decimal?
        Public Property AnchoCm As Decimal?
        Public Property ProfundidadCm As Decimal?
        Public Property Color As String
        Public Property PesoGramos As Decimal?
        Public Property ImagenUrl As String
        Public Property UnidadMedidaId As Integer?
        Public Property UnidadMedidaNombre As String
        Public Property CategoriaId As Integer
        Public Property CategoriaNombre As String
        Public Property LoteProducto As String
        Public Property Estado As String
    End Class
End Namespace
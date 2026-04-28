Option Strict On
Option Explicit On

Namespace Inventario
    Public Class Categoria
        Public Property CategoriaId As Integer
        Public Property Nombre As String
        Public Property Descripcion As String
        Public Property CategoriaPadreId As Integer?
        Public Property CreatedAt As DateTime?
        Public Property UpdatedAt As DateTime?
        Public Property Estado As String
    End Class
End Namespace
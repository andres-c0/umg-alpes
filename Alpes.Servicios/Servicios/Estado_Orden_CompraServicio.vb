Option Strict On
Option Explicit On

Imports Alpes.Datos.Repositorios
Imports Alpes.Entidades.Compras

Namespace Servicios
    Public Class Estado_Orden_CompraServicio

        Private ReadOnly _datos As Estado_Orden_CompraDatos

        Public Sub New()
            _datos = New Estado_Orden_CompraDatos()
        End Sub

        Public Function Insertar(ByVal entidad As Estado_Orden_Compra) As Integer
            Return _datos.Insertar(entidad)
        End Function

        Public Sub Actualizar(ByVal entidad As Estado_Orden_Compra)
            _datos.Actualizar(entidad)
        End Sub

        Public Sub Eliminar(ByVal id As Integer)
            _datos.Eliminar(id)
        End Sub

        Public Function ObtenerPorId(ByVal id As Integer) As Estado_Orden_Compra
            Return _datos.ObtenerPorId(id)
        End Function

        Public Function Listar() As List(Of Estado_Orden_Compra)
            Return _datos.Listar()
        End Function

        Public Function Buscar(ByVal criterio As String, ByVal valor As String) As List(Of Estado_Orden_Compra)
            Return _datos.Buscar(criterio, valor)
        End Function

    End Class
End Namespace
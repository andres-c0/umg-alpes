Option Strict On
Option Explicit On

Imports Alpes.Datos.Repositorios
Imports Alpes.Entidades.Ventas

Namespace Servicios
    Public Class ResenaComentarioServicio

        Private ReadOnly _datos As ResenaComentarioDatos

        Public Sub New()
            _datos = New ResenaComentarioDatos()
        End Sub

        Public Function Insertar(ByVal entidad As ResenaComentario) As Integer
            Return _datos.Insertar(entidad)
        End Function

        Public Sub Actualizar(ByVal entidad As ResenaComentario)
            _datos.Actualizar(entidad)
        End Sub

        Public Sub Eliminar(ByVal id As Integer)
            _datos.Eliminar(id)
        End Sub

        Public Function ObtenerPorId(ByVal id As Integer) As ResenaComentario
            Return _datos.ObtenerPorId(id)
        End Function

        Public Function Listar() As List(Of ResenaComentario)
            Return _datos.Listar()
        End Function

        Public Function Buscar(ByVal valor As String) As List(Of ResenaComentario)
            Return _datos.Buscar(valor)
        End Function

        Public Function Buscar(ByVal criterio As String, ByVal valor As String) As List(Of ResenaComentario)
            Return _datos.Buscar(criterio, valor)
        End Function

        Public Function ObtenerPorCliente(ByVal cliId As Integer) As List(Of ResenaComentario)
            Return _datos.Buscar("CLI_ID", cliId.ToString())
        End Function

        Public Function ObtenerPorProducto(ByVal productoId As Integer) As List(Of ResenaComentario)
            Return _datos.Buscar("PRODUCTO_ID", productoId.ToString())
        End Function

    End Class
End Namespace
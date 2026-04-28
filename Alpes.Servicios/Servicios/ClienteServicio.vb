Option Strict On
Option Explicit On

Imports Alpes.Datos.Repositorios
Imports Alpes.Entidades.Clientes

Namespace Servicios
    Public Class ClienteServicio

        Private ReadOnly _datos As ClienteDatos

        Public Sub New()
            _datos = New ClienteDatos()
        End Sub

        Public Function Insertar(ByVal entidad As Cliente) As Integer
            Return _datos.Insertar(entidad)
        End Function

        Public Sub Actualizar(ByVal entidad As Cliente)
            _datos.Actualizar(entidad)
        End Sub

        Public Sub Eliminar(ByVal id As Integer)
            _datos.Eliminar(id)
        End Sub

        Public Function ObtenerPorId(ByVal id As Integer) As Cliente
            Return _datos.ObtenerPorId(id)
        End Function

        Public Function Listar() As List(Of Cliente)
            Return _datos.Listar()
        End Function

        Public Function Buscar(ByVal criterio As String, ByVal valor As String) As List(Of Cliente)
            Return _datos.Buscar(criterio, valor)
        End Function

    End Class
End Namespace
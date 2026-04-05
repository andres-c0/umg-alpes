Option Strict On
Option Explicit On

Imports Alpes.Datos.Repositorios
Imports Alpes.Entidades.Ventas

Namespace Servicios
    Public Class DevolucionServicio

        Private ReadOnly _datos As DevolucionDatos

        Public Sub New()
            _datos = New DevolucionDatos()
        End Sub

        Public Function Listar() As List(Of Devolucion)
            Return _datos.Listar()
        End Function

        Public Function ObtenerPorId(ByVal id As Integer) As Devolucion
            Return _datos.ObtenerPorId(id)
        End Function

        Public Function Buscar(ByVal criterio As String, ByVal valor As String) As List(Of Devolucion)
            Return _datos.Buscar(criterio, valor)
        End Function

        Public Function Insertar(ByVal entidad As Devolucion) As Integer
            Return _datos.Insertar(entidad)
        End Function

        Public Sub Actualizar(ByVal entidad As Devolucion)
            _datos.Actualizar(entidad)
        End Sub

        Public Sub Eliminar(ByVal id As Integer)
            _datos.Eliminar(id)
        End Sub

    End Class
End Namespace

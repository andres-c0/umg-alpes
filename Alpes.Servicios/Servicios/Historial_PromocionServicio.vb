Option Strict On
Option Explicit On

Imports Alpes.Datos.Repositorios
Imports Alpes.Entidades.Ventas

Namespace Servicios
    Public Class HistorialPromocionServicio

        Private ReadOnly _datos As HistorialPromocionDatos

        Public Sub New()
            _datos = New HistorialPromocionDatos()
        End Sub

        Public Function Insertar(ByVal entidad As HistorialPromocion) As Integer
            Return _datos.Insertar(entidad)
        End Function

        Public Sub Actualizar(ByVal entidad As HistorialPromocion)
            _datos.Actualizar(entidad)
        End Sub

        Public Sub Eliminar(ByVal id As Integer)
            _datos.Eliminar(id)
        End Sub

        Public Function ObtenerPorId(ByVal id As Integer) As HistorialPromocion
            Return _datos.ObtenerPorId(id)
        End Function

        Public Function Listar() As List(Of HistorialPromocion)
            Return _datos.Listar()
        End Function

        Public Function Buscar(ByVal criterio As String, ByVal valor As String) As List(Of HistorialPromocion)
            Return _datos.Buscar(criterio, valor)
        End Function

    End Class
End Namespace
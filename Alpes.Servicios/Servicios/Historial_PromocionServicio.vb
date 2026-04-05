Option Strict On
Option Explicit On

Imports Alpes.Datos.Repositorios
Imports Alpes.Entidades.Ventas

Namespace Servicios
    Public Class Historial_PromocionServicio

        Private ReadOnly _datos As Historial_PromocionDatos

        Public Sub New()
            _datos = New Historial_PromocionDatos()
        End Sub

        Public Function Listar() As List(Of Historial_Promocion)
            Return _datos.Listar()
        End Function

        Public Function ObtenerPorId(ByVal id As Integer) As Historial_Promocion
            Return _datos.ObtenerPorId(id)
        End Function

        Public Function Buscar(ByVal criterio As String, ByVal valor As String) As List(Of Historial_Promocion)
            Return _datos.Buscar(criterio, valor)
        End Function

        Public Function Insertar(ByVal entidad As Historial_Promocion) As Integer
            Return _datos.Insertar(entidad)
        End Function

        Public Sub Actualizar(ByVal entidad As Historial_Promocion)
            _datos.Actualizar(entidad)
        End Sub

        Public Sub Eliminar(ByVal id As Integer)
            _datos.Eliminar(id)
        End Sub

    End Class
End Namespace
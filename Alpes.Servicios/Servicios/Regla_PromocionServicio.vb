Option Strict On
Option Explicit On

Imports Alpes.Datos.Repositorios
Imports Alpes.Entidades.Ventas

Namespace Servicios
    Public Class ReglaPromocionServicio

        Private ReadOnly _datos As ReglaPromocionDatos

        Public Sub New()
            _datos = New ReglaPromocionDatos()
        End Sub

        Public Function Insertar(ByVal entidad As ReglaPromocion) As Integer
            Return _datos.Insertar(entidad)
        End Function

        Public Sub Actualizar(ByVal entidad As ReglaPromocion)
            _datos.Actualizar(entidad)
        End Sub

        Public Sub Eliminar(ByVal id As Integer)
            _datos.Eliminar(id)
        End Sub

        Public Function ObtenerPorId(ByVal id As Integer) As ReglaPromocion
            Return _datos.ObtenerPorId(id)
        End Function

        Public Function Listar() As List(Of ReglaPromocion)
            Return _datos.Listar()
        End Function

        Public Function Buscar(ByVal criterio As String, ByVal valor As String) As List(Of ReglaPromocion)
            Return _datos.Buscar(criterio, valor)
        End Function

    End Class
End Namespace
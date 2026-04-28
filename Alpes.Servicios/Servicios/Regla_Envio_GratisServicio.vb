Option Strict On
Option Explicit On

Imports Alpes.Datos.Repositorios
Imports Alpes.Entidades.Ventas

Namespace Servicios
    Public Class ReglaEnvioGratisServicio

        Private ReadOnly _datos As ReglaEnvioGratisDatos

        Public Sub New()
            _datos = New ReglaEnvioGratisDatos()
        End Sub

        Public Function Insertar(ByVal entidad As ReglaEnvioGratis) As Integer
            Return _datos.Insertar(entidad)
        End Function

        Public Sub Actualizar(ByVal entidad As ReglaEnvioGratis)
            _datos.Actualizar(entidad)
        End Sub

        Public Sub Eliminar(ByVal id As Integer)
            _datos.Eliminar(id)
        End Sub

        Public Function ObtenerPorId(ByVal id As Integer) As ReglaEnvioGratis
            Return _datos.ObtenerPorId(id)
        End Function

        Public Function Listar() As List(Of ReglaEnvioGratis)
            Return _datos.Listar()
        End Function

        Public Function Buscar(ByVal criterio As String, ByVal valor As String) As List(Of ReglaEnvioGratis)
            Return _datos.Buscar(criterio, valor)
        End Function

    End Class
End Namespace
Option Strict On
Option Explicit On

Imports Alpes.Datos.Repositorios
Imports Alpes.Entidades.Envios

Namespace Servicios
    Public Class Regla_Envio_GratisServicio

        Private ReadOnly _datos As Regla_Envio_GratisDatos

        Public Sub New()
            _datos = New Regla_Envio_GratisDatos()
        End Sub

        Public Function Listar() As List(Of Regla_Envio_Gratis)
            Return _datos.Listar()
        End Function

        Public Function ObtenerPorId(ByVal id As Integer) As Regla_Envio_Gratis
            Return _datos.ObtenerPorId(id)
        End Function

        Public Function Buscar(ByVal criterio As String, ByVal valor As String) As List(Of Regla_Envio_Gratis)
            Return _datos.Buscar(criterio, valor)
        End Function

        Public Function Insertar(ByVal entidad As Regla_Envio_Gratis) As Integer
            Return _datos.Insertar(entidad)
        End Function

        Public Sub Actualizar(ByVal entidad As Regla_Envio_Gratis)
            _datos.Actualizar(entidad)
        End Sub

        Public Sub Eliminar(ByVal id As Integer)
            _datos.Eliminar(id)
        End Sub

    End Class
End Namespace
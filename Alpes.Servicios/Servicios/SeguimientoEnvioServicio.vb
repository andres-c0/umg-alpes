Option Strict On
Option Explicit On

Imports Alpes.Datos.Repositorios
Imports Alpes.Entidades.Envios

Namespace Servicios
    Public Class SeguimientoEnvioServicio

        Private ReadOnly _datos As SeguimientoEnvioDatos

        Public Sub New()
            _datos = New SeguimientoEnvioDatos()
        End Sub

        Public Function Insertar(ByVal entidad As SeguimientoEnvio) As Integer
            Return _datos.Insertar(entidad)
        End Function

        Public Sub Actualizar(ByVal entidad As SeguimientoEnvio)
            _datos.Actualizar(entidad)
        End Sub

        Public Sub Eliminar(ByVal id As Integer)
            _datos.Eliminar(id)
        End Sub

        Public Function ObtenerPorId(ByVal id As Integer) As SeguimientoEnvio
            Return _datos.ObtenerPorId(id)
        End Function

        Public Function Listar() As List(Of SeguimientoEnvio)
            Return _datos.Listar()
        End Function

        Public Function Buscar(ByVal criterio As String, ByVal valor As String) As List(Of SeguimientoEnvio)
            Return _datos.Buscar(criterio, valor)
        End Function

    End Class
End Namespace
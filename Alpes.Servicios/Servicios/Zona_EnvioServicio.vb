Option Strict On
Option Explicit On

Imports Alpes.Datos.Repositorios
Imports Alpes.Entidades.Envios

Namespace Servicios
    Public Class Zona_EnvioServicio

        Private ReadOnly _datos As Zona_EnvioDatos

        Public Sub New()
            _datos = New Zona_EnvioDatos()
        End Sub

        Public Function Insertar(ByVal entidad As Zona_Envio) As Integer
            Return _datos.Insertar(entidad)
        End Function

        Public Sub Actualizar(ByVal entidad As Zona_Envio)
            _datos.Actualizar(entidad)
        End Sub

        Public Sub Eliminar(ByVal id As Integer)
            _datos.Eliminar(id)
        End Sub

        Public Function ObtenerPorId(ByVal id As Integer) As Zona_Envio
            Return _datos.ObtenerPorId(id)
        End Function

        Public Function Listar() As List(Of Zona_Envio)
            Return _datos.Listar()
        End Function

        Public Function Buscar(ByVal criterio As String, ByVal valor As String) As List(Of Zona_Envio)
            Return _datos.Buscar(criterio, valor)
        End Function

    End Class
End Namespace
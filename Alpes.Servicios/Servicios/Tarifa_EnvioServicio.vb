Option Strict On
Option Explicit On

Imports Alpes.Datos.Repositorios
Imports Alpes.Entidades.Ventas

Namespace Servicios
    Public Class Tarifa_EnvioServicio

        Private ReadOnly _datos As Tarifa_EnvioDatos

        Public Sub New()
            _datos = New Tarifa_EnvioDatos()
        End Sub

        Public Function Listar() As List(Of Tarifa_Envio)
            Return _datos.Listar()
        End Function

        Public Function ObtenerPorId(ByVal id As Integer) As Tarifa_Envio
            Return _datos.ObtenerPorId(id)
        End Function

        Public Function Buscar(ByVal criterio As String, ByVal valor As String) As List(Of Tarifa_Envio)
            Return _datos.Buscar(criterio, valor)
        End Function

        Public Function Insertar(ByVal entidad As Tarifa_Envio) As Integer
            Return _datos.Insertar(entidad)
        End Function

        Public Sub Actualizar(ByVal entidad As Tarifa_Envio)
            _datos.Actualizar(entidad)
        End Sub

        Public Sub Eliminar(ByVal id As Integer)
            _datos.Eliminar(id)
        End Sub

    End Class
End Namespace
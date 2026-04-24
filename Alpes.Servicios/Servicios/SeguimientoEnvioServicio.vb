Option Strict On
Option Explicit On

Imports System.Collections.Generic
Imports Alpes.Datos.Repositorios
Imports Alpes.Entidades.Ventas

Namespace Servicios
    Public Class SeguimientoEnvioServicio

        Private ReadOnly _datos As SeguimientoEnvioDatos

        Public Sub New()
            _datos = New SeguimientoEnvioDatos()
        End Sub

        Public Function Insertar(ByVal entidad As SeguimientoEnvio) As Integer
            Return _datos.Insertar(entidad)
        End Function

        Public Function Actualizar(ByVal entidad As SeguimientoEnvio) As Boolean
            Return _datos.Actualizar(entidad)
        End Function

        Public Function Eliminar(ByVal segEnvioId As Integer) As Boolean
            Return _datos.Eliminar(segEnvioId)
        End Function

        Public Function ObtenerPorId(ByVal segEnvioId As Integer) As SeguimientoEnvio
            Return _datos.ObtenerPorId(segEnvioId)
        End Function

        Public Function Listar() As List(Of SeguimientoEnvio)
            Return _datos.Listar()
        End Function

        Public Function Buscar(ByVal criterio As String, ByVal valor As String) As List(Of SeguimientoEnvio)
            Return _datos.Buscar(criterio, valor)
        End Function

    End Class
End Namespace
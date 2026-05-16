Option Strict On
Option Explicit On

Imports System.Collections.Generic
Imports Alpes.Datos.Repositorios
Imports Alpes.Entidades.Ventas

Namespace Servicios
    Public Class EnvioServicio

        Private ReadOnly _datos As EnvioDatos

        Public Sub New()
            _datos = New EnvioDatos()
        End Sub

        Public Function Insertar(ByVal entidad As Envio) As Integer
            Return _datos.Insertar(entidad)
        End Function

        Public Function Actualizar(ByVal entidad As Envio) As Boolean
            Return _datos.Actualizar(entidad)
        End Function

        Public Function Eliminar(ByVal envioId As Integer) As Boolean
            Return _datos.Eliminar(envioId)
        End Function

        Public Function ObtenerPorId(ByVal envioId As Integer) As Envio
            Return _datos.ObtenerPorId(envioId)
        End Function

        Public Function Listar() As List(Of Envio)
            Return _datos.Listar()
        End Function

        Public Function Buscar(ByVal criterio As String, ByVal valor As String) As List(Of Envio)
            Return _datos.Buscar(criterio, valor)
        End Function

    End Class
End Namespace
Option Strict On
Option Explicit On

Imports System.Collections.Generic
Imports Alpes.Datos.Repositorios
Imports Alpes.Entidades.Ventas

Namespace Servicios
    Public Class TarjetaClienteServicio

        Private ReadOnly _datos As TarjetaClienteDatos

        Public Sub New()
            _datos = New TarjetaClienteDatos()
        End Sub

        Public Sub Insertar(ByVal entidad As TarjetaCliente)
            _datos.Insertar(entidad)
        End Sub

        Public Function ObtenerPorCliente(ByVal cliId As Integer) As List(Of TarjetaCliente)
            Return _datos.ObtenerPorCliente(cliId)
        End Function

        Public Function ObtenerPorId(ByVal tarjetaClienteId As Integer) As TarjetaCliente
            Return _datos.ObtenerPorId(tarjetaClienteId)
        End Function

        Public Sub MarcarPredeterminada(ByVal tarjetaClienteId As Integer, ByVal cliId As Integer)
            _datos.MarcarPredeterminada(tarjetaClienteId, cliId)
        End Sub

        Public Sub Desactivar(ByVal tarjetaClienteId As Integer)
            _datos.Desactivar(tarjetaClienteId)
        End Sub

    End Class
End Namespace
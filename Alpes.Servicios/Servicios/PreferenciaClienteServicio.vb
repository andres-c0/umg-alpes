Option Strict On
Option Explicit On

Imports System.Collections.Generic
Imports Alpes.Datos.Repositorios
Imports Alpes.Entidades.Ventas

Namespace Servicios
    Public Class PreferenciaClienteServicio

        Private ReadOnly _datos As PreferenciaClienteDatos

        Public Sub New()
            _datos = New PreferenciaClienteDatos()
        End Sub

        Public Function Insertar(ByVal entidad As PreferenciaCliente) As Integer
            Return _datos.Insertar(entidad)
        End Function

        Public Function Actualizar(ByVal entidad As PreferenciaCliente) As Boolean
            Return _datos.Actualizar(entidad)
        End Function

        Public Function Eliminar(ByVal preferenciaId As Integer) As Boolean
            Return _datos.Eliminar(preferenciaId)
        End Function

        Public Function ObtenerPorId(ByVal preferenciaId As Integer) As PreferenciaCliente
            Return _datos.ObtenerPorId(preferenciaId)
        End Function

        Public Function Listar() As List(Of PreferenciaCliente)
            Return _datos.Listar()
        End Function

        Public Function Buscar(ByVal criterio As String, ByVal valor As String) As List(Of PreferenciaCliente)
            Return _datos.Buscar(criterio, valor)
        End Function

    End Class
End Namespace
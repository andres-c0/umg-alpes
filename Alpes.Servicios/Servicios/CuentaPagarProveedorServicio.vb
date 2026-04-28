Option Strict On
Option Explicit On

Imports System.Collections.Generic
Imports Alpes.Datos.Repositorios
Imports Alpes.Entidades.Compras

Namespace Servicios
    Public Class CuentaPagarProveedorServicio

        Private ReadOnly _datos As CuentaPagarProveedorDatos

        Public Sub New()
            _datos = New CuentaPagarProveedorDatos()
        End Sub

        Public Function Insertar(ByVal entidad As CuentaPagarProveedor) As Integer
            Return _datos.Insertar(entidad)
        End Function

        Public Function Actualizar(ByVal entidad As CuentaPagarProveedor) As Boolean
            Return _datos.Actualizar(entidad)
        End Function

        Public Function Eliminar(ByVal cuentaPagarId As Integer) As Boolean
            Return _datos.Eliminar(cuentaPagarId)
        End Function

        Public Function ObtenerPorId(ByVal cuentaPagarId As Integer) As CuentaPagarProveedor
            Return _datos.ObtenerPorId(cuentaPagarId)
        End Function

        Public Function Listar() As List(Of CuentaPagarProveedor)
            Return _datos.Listar()
        End Function

        Public Function Buscar(ByVal criterio As String, ByVal valor As String) As List(Of CuentaPagarProveedor)
            Return _datos.Buscar(criterio, valor)
        End Function

    End Class
End Namespace
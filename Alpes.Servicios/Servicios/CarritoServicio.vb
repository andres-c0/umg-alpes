Option Strict On
Option Explicit On

Imports System.Collections.Generic
Imports Alpes.Datos.Repositorios
Imports Alpes.Entidades.Ventas

Namespace Servicios
    Public Class CarritoServicio

        Private ReadOnly _datos As CarritoDatos

        Public Sub New()
            _datos = New CarritoDatos()
        End Sub

        Public Function Insertar(ByVal entidad As Carrito) As Integer
            Return _datos.Insertar(entidad)
        End Function

        Public Function Actualizar(ByVal entidad As Carrito) As Boolean
            Return _datos.Actualizar(entidad)
        End Function

        Public Function Eliminar(ByVal carritoId As Integer) As Boolean
            Return _datos.Eliminar(carritoId)
        End Function

        Public Function ObtenerPorId(ByVal carritoId As Integer) As Carrito
            Return _datos.ObtenerPorId(carritoId)
        End Function

        Public Function Listar() As List(Of Carrito)
            Return _datos.Listar()
        End Function

        Public Function Buscar(ByVal criterio As String, ByVal valor As String) As List(Of Carrito)
            Return _datos.Buscar(criterio, valor)
        End Function

    End Class
End Namespace
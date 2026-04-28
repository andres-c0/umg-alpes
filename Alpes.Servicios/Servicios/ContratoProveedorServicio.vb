Option Strict On
Option Explicit On

Imports System.Collections.Generic
Imports Alpes.Datos.Repositorios
Imports Alpes.Entidades.Compras

Namespace Servicios
    Public Class ContratoProveedorServicio

        Private ReadOnly _datos As ContratoProveedorDatos

        Public Sub New()
            _datos = New ContratoProveedorDatos()
        End Sub

        Public Function Insertar(ByVal entidad As ContratoProveedor) As Integer
            Return _datos.Insertar(entidad)
        End Function

        Public Function Actualizar(ByVal entidad As ContratoProveedor) As Boolean
            Return _datos.Actualizar(entidad)
        End Function

        Public Function Eliminar(ByVal contratoProveedorId As Integer) As Boolean
            Return _datos.Eliminar(contratoProveedorId)
        End Function

        Public Function ObtenerPorId(ByVal contratoProveedorId As Integer) As ContratoProveedor
            Return _datos.ObtenerPorId(contratoProveedorId)
        End Function

        Public Function Listar() As List(Of ContratoProveedor)
            Return _datos.Listar()
        End Function

        Public Function Buscar(ByVal criterio As String, ByVal valor As String) As List(Of ContratoProveedor)
            Return _datos.Buscar(criterio, valor)
        End Function

    End Class
End Namespace
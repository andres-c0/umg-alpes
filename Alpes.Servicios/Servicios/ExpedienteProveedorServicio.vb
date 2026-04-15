Option Strict On
Option Explicit On

Imports System.Collections.Generic
Imports Alpes.Datos.Repositorios
Imports Alpes.Entidades.Compras

Namespace Servicios
    Public Class ExpedienteProveedorServicio

        Private ReadOnly _datos As ExpedienteProveedorDatos

        Public Sub New()
            _datos = New ExpedienteProveedorDatos()
        End Sub

        Public Function Insertar(ByVal entidad As ExpedienteProveedor) As Integer
            Return _datos.Insertar(entidad)
        End Function

        Public Function Actualizar(ByVal entidad As ExpedienteProveedor) As Boolean
            Return _datos.Actualizar(entidad)
        End Function

        Public Function Eliminar(ByVal expedienteProveedorId As Integer) As Boolean
            Return _datos.Eliminar(expedienteProveedorId)
        End Function

        Public Function ObtenerPorId(ByVal expedienteProveedorId As Integer) As ExpedienteProveedor
            Return _datos.ObtenerPorId(expedienteProveedorId)
        End Function

        Public Function Listar() As List(Of ExpedienteProveedor)
            Return _datos.Listar()
        End Function

        Public Function Buscar(ByVal criterio As String, ByVal valor As String) As List(Of ExpedienteProveedor)
            Return _datos.Buscar(criterio, valor)
        End Function

    End Class
End Namespace
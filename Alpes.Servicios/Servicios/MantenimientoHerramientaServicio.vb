Option Strict On
Option Explicit On

Imports System.Collections.Generic
Imports Alpes.Datos.Repositorios
Imports Alpes.Entidades.Produccion

Namespace Servicios
    Public Class MantenimientoHerramientaServicio

        Private ReadOnly _datos As MantenimientoHerramientaDatos

        Public Sub New()
            _datos = New MantenimientoHerramientaDatos()
        End Sub

        Public Function Insertar(ByVal entidad As MantenimientoHerramienta) As Integer
            Return _datos.Insertar(entidad)
        End Function

        Public Function Actualizar(ByVal entidad As MantenimientoHerramienta) As Boolean
            Return _datos.Actualizar(entidad)
        End Function

        Public Function Eliminar(ByVal mantenimientoId As Integer) As Boolean
            Return _datos.Eliminar(mantenimientoId)
        End Function

        Public Function ObtenerPorId(ByVal mantenimientoId As Integer) As MantenimientoHerramienta
            Return _datos.ObtenerPorId(mantenimientoId)
        End Function

        Public Function Listar() As List(Of MantenimientoHerramienta)
            Return _datos.Listar()
        End Function

        Public Function Buscar(ByVal valor As String) As List(Of MantenimientoHerramienta)
            Return _datos.Buscar(valor)
        End Function

    End Class
End Namespace
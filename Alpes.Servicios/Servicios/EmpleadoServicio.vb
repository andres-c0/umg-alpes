Option Strict On
Option Explicit On

Imports System.Collections.Generic
Imports Alpes.Datos.Repositorios
Imports Alpes.Entidades.RecursosHumanos

Namespace Servicios
    Public Class EmpleadoServicio

        Private ReadOnly _datos As EmpleadoDatos

        Public Sub New()
            _datos = New EmpleadoDatos()
        End Sub

        Public Function Insertar(ByVal entidad As Empleado) As Integer
            Return _datos.Insertar(entidad)
        End Function

        Public Sub Actualizar(ByVal entidad As Empleado)
            _datos.Actualizar(entidad)
        End Sub

        Public Sub Eliminar(ByVal id As Integer)
            _datos.Eliminar(id)
        End Sub

        Public Function ObtenerPorId(ByVal id As Integer) As Empleado
            Return _datos.ObtenerPorId(id)
        End Function

        Public Function Listar() As List(Of Empleado)
            Return _datos.Listar()
        End Function

    End Class
End Namespace
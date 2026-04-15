/* ============================================
   PACKAGE SPEC
   ============================================ */

CREATE OR REPLACE PACKAGE PKG_ROL_EMPLEADO AS

  PROCEDURE SP_INSERTAR_ROL(
    P_NOMBRE        IN ROL_EMPLEADO.NOMBRE%TYPE,
    P_DESCRIPCION   IN ROL_EMPLEADO.DESCRIPCION%TYPE,
    P_ROL_ID        OUT ROL_EMPLEADO.ROL_ID%TYPE
  );

  PROCEDURE SP_ACTUALIZAR_ROL(
    P_ROL_ID        IN ROL_EMPLEADO.ROL_ID%TYPE,
    P_NOMBRE        IN ROL_EMPLEADO.NOMBRE%TYPE,
    P_DESCRIPCION   IN ROL_EMPLEADO.DESCRIPCION%TYPE
  );

  PROCEDURE SP_ELIMINAR_ROL(
    P_ROL_ID IN ROL_EMPLEADO.ROL_ID%TYPE
  );

  PROCEDURE SP_OBTENER_ROL(
    P_ROL_ID IN ROL_EMPLEADO.ROL_ID%TYPE,
    P_CURSOR OUT SYS_REFCURSOR
  );

  PROCEDURE SP_LISTAR_ROLES(
    P_CURSOR OUT SYS_REFCURSOR
  );

END PKG_ROL_EMPLEADO;
/

/* ============================================
   PACKAGE BODY
   ============================================ */

PACKAGE BODY PKG_ROL_EMPLEADO AS

  PROCEDURE INSERTAR(
    p_nombre IN VARCHAR2,
    p_descripcion IN VARCHAR2,
    p_estado IN VARCHAR2,
    p_rol_empleado_id OUT NUMBER
  ) IS
  BEGIN
    INSERT INTO ROL_EMPLEADO (
      nombre,
      descripcion,
      estado,
      created_at
    )
    VALUES (
      p_nombre,
      p_descripcion,
      p_estado,
      SYSTIMESTAMP
    )
    RETURNING rol_empleado_id INTO p_rol_empleado_id;
  END INSERTAR;



  PROCEDURE ACTUALIZAR(
    p_rol_empleado_id IN NUMBER,
    p_nombre IN VARCHAR2,
    p_descripcion IN VARCHAR2,
    p_estado IN VARCHAR2
  ) IS
  BEGIN
    UPDATE ROL_EMPLEADO
       SET nombre = p_nombre,
           descripcion = p_descripcion,
           estado = p_estado,
           updated_at = SYSTIMESTAMP
     WHERE rol_empleado_id = p_rol_empleado_id;
  END ACTUALIZAR;



  PROCEDURE ELIMINAR(
    p_rol_empleado_id IN NUMBER
  ) IS
  BEGIN
    UPDATE ROL_EMPLEADO
       SET estado = 'INACTIVO',
           updated_at = SYSTIMESTAMP
     WHERE rol_empleado_id = p_rol_empleado_id;
  END ELIMINAR;



  PROCEDURE OBTENER_POR_ID(
    p_rol_empleado_id IN NUMBER,
    p_resultado OUT SYS_REFCURSOR
  ) IS
  BEGIN
    OPEN p_resultado FOR
      SELECT rol_empleado_id, nombre, descripcion, estado, created_at, updated_at
        FROM ROL_EMPLEADO
       WHERE rol_empleado_id = p_rol_empleado_id;
  END OBTENER_POR_ID;



  PROCEDURE LISTAR(
    p_resultado OUT SYS_REFCURSOR
  ) IS
  BEGIN
    OPEN p_resultado FOR
      SELECT rol_empleado_id, nombre, descripcion, estado, created_at, updated_at
        FROM ROL_EMPLEADO
       ORDER BY rol_empleado_id;
  END LISTAR;

END PKG_ROL_EMPLEADO;
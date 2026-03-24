CREATE OR REPLACE PACKAGE PKG_ESTADO_ORDEN AS

  PROCEDURE SP_INSERTAR(
    P_CODIGO IN Estado_Orden.codigo%TYPE,
    P_DESCRIPCION IN Estado_Orden.descripcion%TYPE,
    P_ESTADO IN Estado_Orden.estado%TYPE,
    P_ID OUT Estado_Orden.estado_orden_id%TYPE
  );

  PROCEDURE SP_ACTUALIZAR(
    P_ESTADO_ORDEN_ID IN Estado_Orden.estado_orden_id%TYPE,
    P_CODIGO IN Estado_Orden.codigo%TYPE,
    P_DESCRIPCION IN Estado_Orden.descripcion%TYPE,
    P_ESTADO IN Estado_Orden.estado%TYPE
  );

  PROCEDURE SP_ELIMINAR(
    P_ID IN Estado_Orden.estado_orden_id%TYPE
  );

  PROCEDURE SP_OBTENER(
    P_ID     IN Estado_Orden.estado_orden_id%TYPE,
    P_CURSOR OUT SYS_REFCURSOR
  );

  PROCEDURE SP_LISTAR(
    P_CURSOR OUT SYS_REFCURSOR
  );

  PROCEDURE SP_BUSCAR(
    P_VALOR  IN Estado_Orden.codigo%TYPE,
    P_CURSOR OUT SYS_REFCURSOR
  );

END PKG_ESTADO_ORDEN;
/
CREATE OR REPLACE PACKAGE BODY PKG_ESTADO_ORDEN AS

  PROCEDURE SP_INSERTAR(
    P_CODIGO IN Estado_Orden.codigo%TYPE,
    P_DESCRIPCION IN Estado_Orden.descripcion%TYPE,
    P_ESTADO IN Estado_Orden.estado%TYPE,
    P_ID OUT Estado_Orden.estado_orden_id%TYPE
  ) AS
  BEGIN

    INSERT INTO Estado_Orden (
      codigo,
      descripcion,
      estado,
      created_at
    ) VALUES (
      P_CODIGO,
      P_DESCRIPCION,
      NVL(P_ESTADO, 'ACTIVO'),
      SYSTIMESTAMP
    )
    RETURNING estado_orden_id INTO P_ID;
  END SP_INSERTAR;

  PROCEDURE SP_ACTUALIZAR(
    P_ESTADO_ORDEN_ID IN Estado_Orden.estado_orden_id%TYPE,
    P_CODIGO IN Estado_Orden.codigo%TYPE,
    P_DESCRIPCION IN Estado_Orden.descripcion%TYPE,
    P_ESTADO IN Estado_Orden.estado%TYPE
  ) AS
  BEGIN

    UPDATE Estado_Orden
       SET codigo = P_CODIGO,
         descripcion = P_DESCRIPCION,
         estado = P_ESTADO,
         updated_at = SYSTIMESTAMP
     WHERE estado_orden_id = P_ESTADO_ORDEN_ID AND estado = 'ACTIVO';

    IF SQL%ROWCOUNT = 0 THEN
      RAISE_APPLICATION_ERROR(-20002, 'No se encontró el registro a actualizar en Estado_Orden');
    END IF;
  END SP_ACTUALIZAR;

  PROCEDURE SP_ELIMINAR(
    P_ID IN Estado_Orden.estado_orden_id%TYPE
  ) AS
  BEGIN
    UPDATE Estado_Orden
       SET estado = 'INACTIVO',
           updated_at = SYSTIMESTAMP
     WHERE estado_orden_id = P_ID
       AND estado = 'ACTIVO';

    IF SQL%ROWCOUNT = 0 THEN
      RAISE_APPLICATION_ERROR(-20003, 'No se encontró el registro a eliminar en Estado_Orden');
    END IF;
  END SP_ELIMINAR;

  PROCEDURE SP_OBTENER(
    P_ID     IN Estado_Orden.estado_orden_id%TYPE,
    P_CURSOR OUT SYS_REFCURSOR
  ) AS
  BEGIN
    OPEN P_CURSOR FOR
      SELECT estado_orden_id,
             codigo,
             descripcion,
             created_at,
             updated_at,
             estado
        FROM Estado_Orden
       WHERE estado_orden_id = P_ID AND estado = 'ACTIVO';
  END SP_OBTENER;

  PROCEDURE SP_LISTAR(
    P_CURSOR OUT SYS_REFCURSOR
  ) AS
  BEGIN
    OPEN P_CURSOR FOR
      SELECT estado_orden_id,
             codigo,
             descripcion,
             created_at,
             updated_at,
             estado
        FROM Estado_Orden
       WHERE estado = 'ACTIVO'
       ORDER BY estado_orden_id DESC;
  END SP_LISTAR;

  PROCEDURE SP_BUSCAR(
    P_VALOR  IN Estado_Orden.codigo%TYPE,
    P_CURSOR OUT SYS_REFCURSOR
  ) AS
  BEGIN
    OPEN P_CURSOR FOR
      SELECT estado_orden_id,
             codigo,
             descripcion,
             created_at,
             updated_at,
             estado
        FROM Estado_Orden
       WHERE UPPER(NVL(TO_CHAR(codigo), '')) LIKE '%' || UPPER(TO_CHAR(P_VALOR)) || '%' AND estado = 'ACTIVO'
       ORDER BY estado_orden_id DESC;
  END SP_BUSCAR;

END PKG_ESTADO_ORDEN;
/

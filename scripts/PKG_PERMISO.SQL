CREATE OR REPLACE PACKAGE PKG_PERMISO AS

  PROCEDURE SP_INSERTAR(
    P_CODIGO IN Permiso.codigo%TYPE,
    P_DESCRIPCION IN Permiso.descripcion%TYPE,
    P_MODULO IN Permiso.modulo%TYPE,
    P_ESTADO IN Permiso.estado%TYPE,
    P_ID OUT Permiso.permiso_id%TYPE
  );

  PROCEDURE SP_ACTUALIZAR(
    P_PERMISO_ID IN Permiso.permiso_id%TYPE,
    P_CODIGO IN Permiso.codigo%TYPE,
    P_DESCRIPCION IN Permiso.descripcion%TYPE,
    P_MODULO IN Permiso.modulo%TYPE,
    P_ESTADO IN Permiso.estado%TYPE
  );

  PROCEDURE SP_ELIMINAR(
    P_ID IN Permiso.permiso_id%TYPE
  );

  PROCEDURE SP_OBTENER(
    P_ID     IN Permiso.permiso_id%TYPE,
    P_CURSOR OUT SYS_REFCURSOR
  );

  PROCEDURE SP_LISTAR(
    P_CURSOR OUT SYS_REFCURSOR
  );

  PROCEDURE SP_BUSCAR(
    P_VALOR  IN Permiso.codigo%TYPE,
    P_CURSOR OUT SYS_REFCURSOR
  );

END PKG_PERMISO;
/
CREATE OR REPLACE PACKAGE BODY PKG_PERMISO AS

  PROCEDURE SP_INSERTAR(
    P_CODIGO IN Permiso.codigo%TYPE,
    P_DESCRIPCION IN Permiso.descripcion%TYPE,
    P_MODULO IN Permiso.modulo%TYPE,
    P_ESTADO IN Permiso.estado%TYPE,
    P_ID OUT Permiso.permiso_id%TYPE
  ) AS
  BEGIN

    INSERT INTO Permiso (
      codigo,
      descripcion,
      modulo,
      estado,
      created_at
    ) VALUES (
      P_CODIGO,
      P_DESCRIPCION,
      P_MODULO,
      NVL(P_ESTADO, 'ACTIVO'),
      SYSTIMESTAMP
    )
    RETURNING permiso_id INTO P_ID;
  END SP_INSERTAR;

  PROCEDURE SP_ACTUALIZAR(
    P_PERMISO_ID IN Permiso.permiso_id%TYPE,
    P_CODIGO IN Permiso.codigo%TYPE,
    P_DESCRIPCION IN Permiso.descripcion%TYPE,
    P_MODULO IN Permiso.modulo%TYPE,
    P_ESTADO IN Permiso.estado%TYPE
  ) AS
  BEGIN

    UPDATE Permiso
       SET codigo = P_CODIGO,
         descripcion = P_DESCRIPCION,
         modulo = P_MODULO,
         estado = P_ESTADO,
         updated_at = SYSTIMESTAMP
     WHERE permiso_id = P_PERMISO_ID AND estado = 'ACTIVO';

    IF SQL%ROWCOUNT = 0 THEN
      RAISE_APPLICATION_ERROR(-20002, 'No se encontró el registro a actualizar en Permiso');
    END IF;
  END SP_ACTUALIZAR;

  PROCEDURE SP_ELIMINAR(
    P_ID IN Permiso.permiso_id%TYPE
  ) AS
  BEGIN
    UPDATE Permiso
       SET estado = 'INACTIVO',
           updated_at = SYSTIMESTAMP
     WHERE permiso_id = P_ID
       AND estado = 'ACTIVO';

    IF SQL%ROWCOUNT = 0 THEN
      RAISE_APPLICATION_ERROR(-20003, 'No se encontró el registro a eliminar en Permiso');
    END IF;
  END SP_ELIMINAR;

  PROCEDURE SP_OBTENER(
    P_ID     IN Permiso.permiso_id%TYPE,
    P_CURSOR OUT SYS_REFCURSOR
  ) AS
  BEGIN
    OPEN P_CURSOR FOR
      SELECT permiso_id,
             codigo,
             descripcion,
             modulo,
             created_at,
             updated_at,
             estado
        FROM Permiso
       WHERE permiso_id = P_ID AND estado = 'ACTIVO';
  END SP_OBTENER;

  PROCEDURE SP_LISTAR(
    P_CURSOR OUT SYS_REFCURSOR
  ) AS
  BEGIN
    OPEN P_CURSOR FOR
      SELECT permiso_id,
             codigo,
             descripcion,
             modulo,
             created_at,
             updated_at,
             estado
        FROM Permiso
       WHERE estado = 'ACTIVO'
       ORDER BY permiso_id DESC;
  END SP_LISTAR;

  PROCEDURE SP_BUSCAR(
    P_VALOR  IN Permiso.codigo%TYPE,
    P_CURSOR OUT SYS_REFCURSOR
  ) AS
  BEGIN
    OPEN P_CURSOR FOR
      SELECT permiso_id,
             codigo,
             descripcion,
             modulo,
             created_at,
             updated_at,
             estado
        FROM Permiso
       WHERE UPPER(NVL(TO_CHAR(codigo), '')) LIKE '%' || UPPER(TO_CHAR(P_VALOR)) || '%' AND estado = 'ACTIVO'
       ORDER BY permiso_id DESC;
  END SP_BUSCAR;

END PKG_PERMISO;
/

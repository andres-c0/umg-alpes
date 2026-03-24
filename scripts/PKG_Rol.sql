CREATE OR REPLACE PACKAGE PKG_ROL AS

  PROCEDURE SP_INSERTAR(
    P_ROL_NOMBRE IN Rol.rol_nombre%TYPE,
    P_DESCRIPCION IN Rol.descripcion%TYPE,
    P_ESTADO IN Rol.estado%TYPE,
    P_ID OUT Rol.rol_id%TYPE
  );

  PROCEDURE SP_ACTUALIZAR(
    P_ROL_ID IN Rol.rol_id%TYPE,
    P_ROL_NOMBRE IN Rol.rol_nombre%TYPE,
    P_DESCRIPCION IN Rol.descripcion%TYPE,
    P_ESTADO IN Rol.estado%TYPE
  );

  PROCEDURE SP_ELIMINAR(
    P_ID IN Rol.rol_id%TYPE
  );

  PROCEDURE SP_OBTENER(
    P_ID     IN Rol.rol_id%TYPE,
    P_CURSOR OUT SYS_REFCURSOR
  );

  PROCEDURE SP_LISTAR(
    P_CURSOR OUT SYS_REFCURSOR
  );

  PROCEDURE SP_BUSCAR(
    P_VALOR  IN Rol.rol_nombre%TYPE,
    P_CURSOR OUT SYS_REFCURSOR
  );

END PKG_ROL;
/
CREATE OR REPLACE PACKAGE BODY PKG_ROL AS

  PROCEDURE SP_INSERTAR(
    P_ROL_NOMBRE IN Rol.rol_nombre%TYPE,
    P_DESCRIPCION IN Rol.descripcion%TYPE,
    P_ESTADO IN Rol.estado%TYPE,
    P_ID OUT Rol.rol_id%TYPE
  ) AS
  BEGIN

    INSERT INTO Rol (
      rol_nombre,
      descripcion,
      estado,
      created_at
    ) VALUES (
      P_ROL_NOMBRE,
      P_DESCRIPCION,
      NVL(P_ESTADO, 'ACTIVO'),
      SYSTIMESTAMP
    )
    RETURNING rol_id INTO P_ID;
  END SP_INSERTAR;

  PROCEDURE SP_ACTUALIZAR(
    P_ROL_ID IN Rol.rol_id%TYPE,
    P_ROL_NOMBRE IN Rol.rol_nombre%TYPE,
    P_DESCRIPCION IN Rol.descripcion%TYPE,
    P_ESTADO IN Rol.estado%TYPE
  ) AS
  BEGIN

    UPDATE Rol
       SET rol_nombre = P_ROL_NOMBRE,
         descripcion = P_DESCRIPCION,
         estado = P_ESTADO,
         updated_at = SYSTIMESTAMP
     WHERE rol_id = P_ROL_ID AND estado = 'ACTIVO';

    IF SQL%ROWCOUNT = 0 THEN
      RAISE_APPLICATION_ERROR(-20002, 'No se encontró el registro a actualizar en Rol');
    END IF;
  END SP_ACTUALIZAR;

  PROCEDURE SP_ELIMINAR(
    P_ID IN Rol.rol_id%TYPE
  ) AS
  BEGIN
    UPDATE Rol
       SET estado = 'INACTIVO',
           updated_at = SYSTIMESTAMP
     WHERE rol_id = P_ID
       AND estado = 'ACTIVO';

    IF SQL%ROWCOUNT = 0 THEN
      RAISE_APPLICATION_ERROR(-20003, 'No se encontró el registro a eliminar en Rol');
    END IF;
  END SP_ELIMINAR;

  PROCEDURE SP_OBTENER(
    P_ID     IN Rol.rol_id%TYPE,
    P_CURSOR OUT SYS_REFCURSOR
  ) AS
  BEGIN
    OPEN P_CURSOR FOR
      SELECT rol_id,
             rol_nombre,
             descripcion,
             created_at,
             updated_at,
             estado
        FROM Rol
       WHERE rol_id = P_ID AND estado = 'ACTIVO';
  END SP_OBTENER;

  PROCEDURE SP_LISTAR(
    P_CURSOR OUT SYS_REFCURSOR
  ) AS
  BEGIN
    OPEN P_CURSOR FOR
      SELECT rol_id,
             rol_nombre,
             descripcion,
             created_at,
             updated_at,
             estado
        FROM Rol
       WHERE estado = 'ACTIVO'
       ORDER BY rol_id DESC;
  END SP_LISTAR;

  PROCEDURE SP_BUSCAR(
    P_VALOR  IN Rol.rol_nombre%TYPE,
    P_CURSOR OUT SYS_REFCURSOR
  ) AS
  BEGIN
    OPEN P_CURSOR FOR
      SELECT rol_id,
             rol_nombre,
             descripcion,
             created_at,
             updated_at,
             estado
        FROM Rol
       WHERE UPPER(NVL(TO_CHAR(rol_nombre), '')) LIKE '%' || UPPER(TO_CHAR(P_VALOR)) || '%' AND estado = 'ACTIVO'
       ORDER BY rol_id DESC;
  END SP_BUSCAR;

END PKG_ROL;
/

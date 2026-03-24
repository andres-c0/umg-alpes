CREATE OR REPLACE PACKAGE PKG_ROL_PERMISO AS

  PROCEDURE SP_INSERTAR(
    P_ROL_ID IN Rol_Permiso.rol_id%TYPE,
    P_PERMISO_ID IN Rol_Permiso.permiso_id%TYPE,
    P_ES_PERMITIDO IN Rol_Permiso.es_permitido%TYPE,
    P_ESTADO IN Rol_Permiso.estado%TYPE,
    P_ID OUT Rol_Permiso.rol_permiso_id%TYPE
  );

  PROCEDURE SP_ACTUALIZAR(
    P_ROL_PERMISO_ID IN Rol_Permiso.rol_permiso_id%TYPE,
    P_ROL_ID IN Rol_Permiso.rol_id%TYPE,
    P_PERMISO_ID IN Rol_Permiso.permiso_id%TYPE,
    P_ES_PERMITIDO IN Rol_Permiso.es_permitido%TYPE,
    P_ESTADO IN Rol_Permiso.estado%TYPE
  );

  PROCEDURE SP_ELIMINAR(
    P_ID IN Rol_Permiso.rol_permiso_id%TYPE
  );

  PROCEDURE SP_OBTENER(
    P_ID     IN Rol_Permiso.rol_permiso_id%TYPE,
    P_CURSOR OUT SYS_REFCURSOR
  );

  PROCEDURE SP_LISTAR(
    P_CURSOR OUT SYS_REFCURSOR
  );

  PROCEDURE SP_BUSCAR(
    P_VALOR  IN Rol_Permiso.es_permitido%TYPE,
    P_CURSOR OUT SYS_REFCURSOR
  );

END PKG_ROL_PERMISO;
/
CREATE OR REPLACE PACKAGE BODY PKG_ROL_PERMISO AS

  PROCEDURE SP_INSERTAR(
    P_ROL_ID IN Rol_Permiso.rol_id%TYPE,
    P_PERMISO_ID IN Rol_Permiso.permiso_id%TYPE,
    P_ES_PERMITIDO IN Rol_Permiso.es_permitido%TYPE,
    P_ESTADO IN Rol_Permiso.estado%TYPE,
    P_ID OUT Rol_Permiso.rol_permiso_id%TYPE
  ) AS
  BEGIN
    DECLARE V_EXISTE NUMBER;
    BEGIN
      SELECT COUNT(*) INTO V_EXISTE FROM Rol WHERE rol_id = P_ROL_ID;
      IF V_EXISTE = 0 THEN
        RAISE_APPLICATION_ERROR(-20001, 'No existe registro relacionado en Rol para rol_id');
      END IF;
    END;
    DECLARE V_EXISTE NUMBER;
    BEGIN
      SELECT COUNT(*) INTO V_EXISTE FROM Permiso WHERE permiso_id = P_PERMISO_ID;
      IF V_EXISTE = 0 THEN
        RAISE_APPLICATION_ERROR(-20001, 'No existe registro relacionado en Permiso para permiso_id');
      END IF;
    END;
    INSERT INTO Rol_Permiso (
      rol_id,
      permiso_id,
      es_permitido,
      estado,
      created_at
    ) VALUES (
      P_ROL_ID,
      P_PERMISO_ID,
      P_ES_PERMITIDO,
      NVL(P_ESTADO, 'ACTIVO'),
      SYSTIMESTAMP
    )
    RETURNING rol_permiso_id INTO P_ID;
  END SP_INSERTAR;

  PROCEDURE SP_ACTUALIZAR(
    P_ROL_PERMISO_ID IN Rol_Permiso.rol_permiso_id%TYPE,
    P_ROL_ID IN Rol_Permiso.rol_id%TYPE,
    P_PERMISO_ID IN Rol_Permiso.permiso_id%TYPE,
    P_ES_PERMITIDO IN Rol_Permiso.es_permitido%TYPE,
    P_ESTADO IN Rol_Permiso.estado%TYPE
  ) AS
  BEGIN
    DECLARE V_EXISTE NUMBER;
    BEGIN
      SELECT COUNT(*) INTO V_EXISTE FROM Rol WHERE rol_id = P_ROL_ID;
      IF V_EXISTE = 0 THEN
        RAISE_APPLICATION_ERROR(-20001, 'No existe registro relacionado en Rol para rol_id');
      END IF;
    END;
    DECLARE V_EXISTE NUMBER;
    BEGIN
      SELECT COUNT(*) INTO V_EXISTE FROM Permiso WHERE permiso_id = P_PERMISO_ID;
      IF V_EXISTE = 0 THEN
        RAISE_APPLICATION_ERROR(-20001, 'No existe registro relacionado en Permiso para permiso_id');
      END IF;
    END;
    UPDATE Rol_Permiso
       SET rol_id = P_ROL_ID,
         permiso_id = P_PERMISO_ID,
         es_permitido = P_ES_PERMITIDO,
         estado = P_ESTADO,
         updated_at = SYSTIMESTAMP
     WHERE rol_permiso_id = P_ROL_PERMISO_ID AND estado = 'ACTIVO';

    IF SQL%ROWCOUNT = 0 THEN
      RAISE_APPLICATION_ERROR(-20002, 'No se encontró el registro a actualizar en Rol_Permiso');
    END IF;
  END SP_ACTUALIZAR;

  PROCEDURE SP_ELIMINAR(
    P_ID IN Rol_Permiso.rol_permiso_id%TYPE
  ) AS
  BEGIN
    UPDATE Rol_Permiso
       SET estado = 'INACTIVO',
           updated_at = SYSTIMESTAMP
     WHERE rol_permiso_id = P_ID
       AND estado = 'ACTIVO';

    IF SQL%ROWCOUNT = 0 THEN
      RAISE_APPLICATION_ERROR(-20003, 'No se encontró el registro a eliminar en Rol_Permiso');
    END IF;
  END SP_ELIMINAR;

  PROCEDURE SP_OBTENER(
    P_ID     IN Rol_Permiso.rol_permiso_id%TYPE,
    P_CURSOR OUT SYS_REFCURSOR
  ) AS
  BEGIN
    OPEN P_CURSOR FOR
      SELECT rol_permiso_id,
             rol_id,
             permiso_id,
             es_permitido,
             created_at,
             updated_at,
             estado
        FROM Rol_Permiso
       WHERE rol_permiso_id = P_ID AND estado = 'ACTIVO';
  END SP_OBTENER;

  PROCEDURE SP_LISTAR(
    P_CURSOR OUT SYS_REFCURSOR
  ) AS
  BEGIN
    OPEN P_CURSOR FOR
      SELECT rol_permiso_id,
             rol_id,
             permiso_id,
             es_permitido,
             created_at,
             updated_at,
             estado
        FROM Rol_Permiso
       WHERE estado = 'ACTIVO'
       ORDER BY rol_permiso_id DESC;
  END SP_LISTAR;

  PROCEDURE SP_BUSCAR(
    P_VALOR  IN Rol_Permiso.es_permitido%TYPE,
    P_CURSOR OUT SYS_REFCURSOR
  ) AS
  BEGIN
    OPEN P_CURSOR FOR
      SELECT rol_permiso_id,
             rol_id,
             permiso_id,
             es_permitido,
             created_at,
             updated_at,
             estado
        FROM Rol_Permiso
       WHERE es_permitido = P_VALOR AND estado = 'ACTIVO'
       ORDER BY rol_permiso_id DESC;
  END SP_BUSCAR;

END PKG_ROL_PERMISO;
/

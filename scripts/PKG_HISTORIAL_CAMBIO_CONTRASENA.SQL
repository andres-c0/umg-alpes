CREATE OR REPLACE PACKAGE PKG_HISTORIAL_CAMBIO_CONTRASENA AS

  PROCEDURE SP_INSERTAR(
    P_USU_ID IN Historial_Cambio_Contrasena.usu_id%TYPE,
    P_CAMBIO_AT IN Historial_Cambio_Contrasena.cambio_at%TYPE,
    P_MOTIVO IN Historial_Cambio_Contrasena.motivo%TYPE,
    P_IP IN Historial_Cambio_Contrasena.ip%TYPE,
    P_ID OUT Historial_Cambio_Contrasena.hc_id%TYPE
  );

  PROCEDURE SP_ACTUALIZAR(
    P_HC_ID IN Historial_Cambio_Contrasena.hc_id%TYPE,
    P_USU_ID IN Historial_Cambio_Contrasena.usu_id%TYPE,
    P_CAMBIO_AT IN Historial_Cambio_Contrasena.cambio_at%TYPE,
    P_MOTIVO IN Historial_Cambio_Contrasena.motivo%TYPE,
    P_IP IN Historial_Cambio_Contrasena.ip%TYPE
  );

  PROCEDURE SP_ELIMINAR(
    P_ID IN Historial_Cambio_Contrasena.hc_id%TYPE
  );

  PROCEDURE SP_OBTENER(
    P_ID     IN Historial_Cambio_Contrasena.hc_id%TYPE,
    P_CURSOR OUT SYS_REFCURSOR
  );

  PROCEDURE SP_LISTAR(
    P_CURSOR OUT SYS_REFCURSOR
  );

  PROCEDURE SP_BUSCAR(
    P_VALOR  IN Historial_Cambio_Contrasena.cambio_at%TYPE,
    P_CURSOR OUT SYS_REFCURSOR
  );

END PKG_HISTORIAL_CAMBIO_CONTRASENA;
/
CREATE OR REPLACE PACKAGE BODY PKG_HISTORIAL_CAMBIO_CONTRASENA AS

  PROCEDURE SP_INSERTAR(
    P_USU_ID IN Historial_Cambio_Contrasena.usu_id%TYPE,
    P_CAMBIO_AT IN Historial_Cambio_Contrasena.cambio_at%TYPE,
    P_MOTIVO IN Historial_Cambio_Contrasena.motivo%TYPE,
    P_IP IN Historial_Cambio_Contrasena.ip%TYPE,
    P_ID OUT Historial_Cambio_Contrasena.hc_id%TYPE
  ) AS
  BEGIN
    DECLARE V_EXISTE NUMBER;
    BEGIN
      SELECT COUNT(*) INTO V_EXISTE FROM Usuario WHERE usu_id = P_USU_ID;
      IF V_EXISTE = 0 THEN
        RAISE_APPLICATION_ERROR(-20001, 'No existe registro relacionado en Usuario para usu_id');
      END IF;
    END;
    INSERT INTO Historial_Cambio_Contrasena (
      usu_id,
      cambio_at,
      motivo,
      ip,
      created_at
    ) VALUES (
      P_USU_ID,
      P_CAMBIO_AT,
      P_MOTIVO,
      P_IP,
      SYSTIMESTAMP
    )
    RETURNING hc_id INTO P_ID;
  END SP_INSERTAR;

  PROCEDURE SP_ACTUALIZAR(
    P_HC_ID IN Historial_Cambio_Contrasena.hc_id%TYPE,
    P_USU_ID IN Historial_Cambio_Contrasena.usu_id%TYPE,
    P_CAMBIO_AT IN Historial_Cambio_Contrasena.cambio_at%TYPE,
    P_MOTIVO IN Historial_Cambio_Contrasena.motivo%TYPE,
    P_IP IN Historial_Cambio_Contrasena.ip%TYPE
  ) AS
  BEGIN
    DECLARE V_EXISTE NUMBER;
    BEGIN
      SELECT COUNT(*) INTO V_EXISTE FROM Usuario WHERE usu_id = P_USU_ID;
      IF V_EXISTE = 0 THEN
        RAISE_APPLICATION_ERROR(-20001, 'No existe registro relacionado en Usuario para usu_id');
      END IF;
    END;
    UPDATE Historial_Cambio_Contrasena
       SET usu_id = P_USU_ID,
         cambio_at = P_CAMBIO_AT,
         motivo = P_MOTIVO,
         ip = P_IP,
         updated_at = SYSTIMESTAMP
     WHERE hc_id = P_HC_ID;

    IF SQL%ROWCOUNT = 0 THEN
      RAISE_APPLICATION_ERROR(-20002, 'No se encontró el registro a actualizar en Historial_Cambio_Contrasena');
    END IF;
  END SP_ACTUALIZAR;

  PROCEDURE SP_ELIMINAR(
    P_ID IN Historial_Cambio_Contrasena.hc_id%TYPE
  ) AS
  BEGIN
    DELETE FROM Historial_Cambio_Contrasena WHERE hc_id = P_ID;

    IF SQL%ROWCOUNT = 0 THEN
      RAISE_APPLICATION_ERROR(-20003, 'No se encontró el registro a eliminar en Historial_Cambio_Contrasena');
    END IF;
  END SP_ELIMINAR;

  PROCEDURE SP_OBTENER(
    P_ID     IN Historial_Cambio_Contrasena.hc_id%TYPE,
    P_CURSOR OUT SYS_REFCURSOR
  ) AS
  BEGIN
    OPEN P_CURSOR FOR
      SELECT hc_id,
             usu_id,
             cambio_at,
             motivo,
             ip,
             created_at,
             updated_at
        FROM Historial_Cambio_Contrasena
       WHERE hc_id = P_ID;
  END SP_OBTENER;

  PROCEDURE SP_LISTAR(
    P_CURSOR OUT SYS_REFCURSOR
  ) AS
  BEGIN
    OPEN P_CURSOR FOR
      SELECT hc_id,
             usu_id,
             cambio_at,
             motivo,
             ip,
             created_at,
             updated_at
        FROM Historial_Cambio_Contrasena
       
       ORDER BY hc_id DESC;
  END SP_LISTAR;

  PROCEDURE SP_BUSCAR(
    P_VALOR  IN Historial_Cambio_Contrasena.cambio_at%TYPE,
    P_CURSOR OUT SYS_REFCURSOR
  ) AS
  BEGIN
    OPEN P_CURSOR FOR
      SELECT hc_id,
             usu_id,
             cambio_at,
             motivo,
             ip,
             created_at,
             updated_at
        FROM Historial_Cambio_Contrasena
       WHERE UPPER(NVL(TO_CHAR(cambio_at), '')) LIKE '%' || UPPER(TO_CHAR(P_VALOR)) || '%'
       ORDER BY hc_id DESC;
  END SP_BUSCAR;

END PKG_HISTORIAL_CAMBIO_CONTRASENA;
/

CREATE OR REPLACE PACKAGE PKG_SESION AS

  PROCEDURE SP_INSERTAR(
    P_USU_ID IN Sesion.usu_id%TYPE,
    P_TOKEN_HASH IN Sesion.token_hash%TYPE,
    P_IP IN Sesion.ip%TYPE,
    P_USER_AGENT IN Sesion.user_agent%TYPE,
    P_INICIO_AT IN Sesion.inicio_at%TYPE,
    P_FIN_AT IN Sesion.fin_at%TYPE,
    P_ID OUT Sesion.sesion_id%TYPE
  );

  PROCEDURE SP_ACTUALIZAR(
    P_SESION_ID IN Sesion.sesion_id%TYPE,
    P_USU_ID IN Sesion.usu_id%TYPE,
    P_TOKEN_HASH IN Sesion.token_hash%TYPE,
    P_IP IN Sesion.ip%TYPE,
    P_USER_AGENT IN Sesion.user_agent%TYPE,
    P_INICIO_AT IN Sesion.inicio_at%TYPE,
    P_FIN_AT IN Sesion.fin_at%TYPE
  );

  PROCEDURE SP_ELIMINAR(
    P_ID IN Sesion.sesion_id%TYPE
  );

  PROCEDURE SP_OBTENER(
    P_ID     IN Sesion.sesion_id%TYPE,
    P_CURSOR OUT SYS_REFCURSOR
  );

  PROCEDURE SP_LISTAR(
    P_CURSOR OUT SYS_REFCURSOR
  );

  PROCEDURE SP_BUSCAR(
    P_VALOR  IN Sesion.token_hash%TYPE,
    P_CURSOR OUT SYS_REFCURSOR
  );

END PKG_SESION;
/
CREATE OR REPLACE PACKAGE BODY PKG_SESION AS

  PROCEDURE SP_INSERTAR(
    P_USU_ID IN Sesion.usu_id%TYPE,
    P_TOKEN_HASH IN Sesion.token_hash%TYPE,
    P_IP IN Sesion.ip%TYPE,
    P_USER_AGENT IN Sesion.user_agent%TYPE,
    P_INICIO_AT IN Sesion.inicio_at%TYPE,
    P_FIN_AT IN Sesion.fin_at%TYPE,
    P_ID OUT Sesion.sesion_id%TYPE
  ) AS
  BEGIN
    DECLARE V_EXISTE NUMBER;
    BEGIN
      SELECT COUNT(*) INTO V_EXISTE FROM Usuario WHERE usu_id = P_USU_ID;
      IF V_EXISTE = 0 THEN
        RAISE_APPLICATION_ERROR(-20001, 'No existe registro relacionado en Usuario para usu_id');
      END IF;
    END;
    INSERT INTO Sesion (
      usu_id,
      token_hash,
      ip,
      user_agent,
      inicio_at,
      fin_at,
      created_at
    ) VALUES (
      P_USU_ID,
      P_TOKEN_HASH,
      P_IP,
      P_USER_AGENT,
      P_INICIO_AT,
      P_FIN_AT,
      SYSTIMESTAMP
    )
    RETURNING sesion_id INTO P_ID;
  END SP_INSERTAR;

  PROCEDURE SP_ACTUALIZAR(
    P_SESION_ID IN Sesion.sesion_id%TYPE,
    P_USU_ID IN Sesion.usu_id%TYPE,
    P_TOKEN_HASH IN Sesion.token_hash%TYPE,
    P_IP IN Sesion.ip%TYPE,
    P_USER_AGENT IN Sesion.user_agent%TYPE,
    P_INICIO_AT IN Sesion.inicio_at%TYPE,
    P_FIN_AT IN Sesion.fin_at%TYPE
  ) AS
  BEGIN
    DECLARE V_EXISTE NUMBER;
    BEGIN
      SELECT COUNT(*) INTO V_EXISTE FROM Usuario WHERE usu_id = P_USU_ID;
      IF V_EXISTE = 0 THEN
        RAISE_APPLICATION_ERROR(-20001, 'No existe registro relacionado en Usuario para usu_id');
      END IF;
    END;
    UPDATE Sesion
       SET usu_id = P_USU_ID,
         token_hash = P_TOKEN_HASH,
         ip = P_IP,
         user_agent = P_USER_AGENT,
         inicio_at = P_INICIO_AT,
         fin_at = P_FIN_AT,
         updated_at = SYSTIMESTAMP
     WHERE sesion_id = P_SESION_ID;

    IF SQL%ROWCOUNT = 0 THEN
      RAISE_APPLICATION_ERROR(-20002, 'No se encontró el registro a actualizar en Sesion');
    END IF;
  END SP_ACTUALIZAR;

  PROCEDURE SP_ELIMINAR(
    P_ID IN Sesion.sesion_id%TYPE
  ) AS
  BEGIN
    DELETE FROM Sesion WHERE sesion_id = P_ID;

    IF SQL%ROWCOUNT = 0 THEN
      RAISE_APPLICATION_ERROR(-20003, 'No se encontró el registro a eliminar en Sesion');
    END IF;
  END SP_ELIMINAR;

  PROCEDURE SP_OBTENER(
    P_ID     IN Sesion.sesion_id%TYPE,
    P_CURSOR OUT SYS_REFCURSOR
  ) AS
  BEGIN
    OPEN P_CURSOR FOR
      SELECT sesion_id,
             usu_id,
             token_hash,
             ip,
             user_agent,
             inicio_at,
             fin_at,
             created_at,
             updated_at
        FROM Sesion
       WHERE sesion_id = P_ID;
  END SP_OBTENER;

  PROCEDURE SP_LISTAR(
    P_CURSOR OUT SYS_REFCURSOR
  ) AS
  BEGIN
    OPEN P_CURSOR FOR
      SELECT sesion_id,
             usu_id,
             token_hash,
             ip,
             user_agent,
             inicio_at,
             fin_at,
             created_at,
             updated_at
        FROM Sesion
       
       ORDER BY sesion_id DESC;
  END SP_LISTAR;

  PROCEDURE SP_BUSCAR(
    P_VALOR  IN Sesion.token_hash%TYPE,
    P_CURSOR OUT SYS_REFCURSOR
  ) AS
  BEGIN
    OPEN P_CURSOR FOR
      SELECT sesion_id,
             usu_id,
             token_hash,
             ip,
             user_agent,
             inicio_at,
             fin_at,
             created_at,
             updated_at
        FROM Sesion
       WHERE UPPER(NVL(TO_CHAR(token_hash), '')) LIKE '%' || UPPER(TO_CHAR(P_VALOR)) || '%'
       ORDER BY sesion_id DESC;
  END SP_BUSCAR;

END PKG_SESION;
/

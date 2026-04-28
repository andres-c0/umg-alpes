CREATE OR REPLACE PACKAGE BODY PKG_USUARIO AS

  PROCEDURE SP_INSERTAR(
    P_USERNAME        IN Usuario.username%TYPE,
    P_PASSWORD_HASH   IN Usuario.password_hash%TYPE,
    P_EMAIL           IN Usuario.email%TYPE,
    P_TELEFONO        IN Usuario.telefono%TYPE,
    P_ROL_ID          IN Usuario.rol_id%TYPE,
    P_CLI_ID          IN Usuario.cli_id%TYPE,
    P_EMP_ID          IN Usuario.emp_id%TYPE,
    P_ULTIMO_LOGIN_AT IN Usuario.ultimo_login_at%TYPE,
    P_BLOQUEADO_HASTA IN Usuario.bloqueado_hasta%TYPE,
    P_ESTADO          IN Usuario.estado%TYPE,
    P_ID             OUT Usuario.usu_id%TYPE
  ) AS
  BEGIN
    DECLARE V_EXISTE NUMBER;
    BEGIN
      SELECT COUNT(*) INTO V_EXISTE FROM Rol WHERE rol_id = P_ROL_ID;
      IF V_EXISTE = 0 THEN
        RAISE_APPLICATION_ERROR(-20001, 'No existe registro relacionado en Rol para rol_id');
      END IF;
    END;

    IF P_CLI_ID IS NOT NULL THEN
      DECLARE V_EXISTE NUMBER;
      BEGIN
        SELECT COUNT(*) INTO V_EXISTE FROM Cliente WHERE cli_id = P_CLI_ID;
        IF V_EXISTE = 0 THEN
          RAISE_APPLICATION_ERROR(-20001, 'No existe registro relacionado en Cliente para cli_id');
        END IF;
      END;
    END IF;

    IF P_EMP_ID IS NOT NULL THEN
      DECLARE V_EXISTE NUMBER;
      BEGIN
        SELECT COUNT(*) INTO V_EXISTE FROM Empleado WHERE emp_id = P_EMP_ID;
        IF V_EXISTE = 0 THEN
          RAISE_APPLICATION_ERROR(-20001, 'No existe registro relacionado en Empleado para emp_id');
        END IF;
      END;
    END IF;

    INSERT INTO Usuario (
      username,
      password_hash,
      email,
      telefono,
      rol_id,
      cli_id,
      emp_id,
      ultimo_login_at,
      bloqueado_hasta,
      estado,
      created_at
    ) VALUES (
      P_USERNAME,
      P_PASSWORD_HASH,
      P_EMAIL,
      P_TELEFONO,
      P_ROL_ID,
      P_CLI_ID,
      P_EMP_ID,
      P_ULTIMO_LOGIN_AT,
      P_BLOQUEADO_HASTA,
      NVL(P_ESTADO, 'ACTIVO'),
      SYSTIMESTAMP
    )
    RETURNING usu_id INTO P_ID;
  END SP_INSERTAR;


  PROCEDURE SP_ACTUALIZAR(
    P_USU_ID          IN Usuario.usu_id%TYPE,
    P_USERNAME        IN Usuario.username%TYPE,
    P_PASSWORD_HASH   IN Usuario.password_hash%TYPE,
    P_EMAIL           IN Usuario.email%TYPE,
    P_TELEFONO        IN Usuario.telefono%TYPE,
    P_ROL_ID          IN Usuario.rol_id%TYPE,
    P_CLI_ID          IN Usuario.cli_id%TYPE,
    P_EMP_ID          IN Usuario.emp_id%TYPE,
    P_ULTIMO_LOGIN_AT IN Usuario.ultimo_login_at%TYPE,
    P_BLOQUEADO_HASTA IN Usuario.bloqueado_hasta%TYPE,
    P_ESTADO          IN Usuario.estado%TYPE
  ) AS
  BEGIN
    DECLARE V_EXISTE NUMBER;
    BEGIN
      SELECT COUNT(*) INTO V_EXISTE FROM Rol WHERE rol_id = P_ROL_ID;
      IF V_EXISTE = 0 THEN
        RAISE_APPLICATION_ERROR(-20001, 'No existe registro relacionado en Rol para rol_id');
      END IF;
    END;

    IF P_CLI_ID IS NOT NULL THEN
      DECLARE V_EXISTE NUMBER;
      BEGIN
        SELECT COUNT(*) INTO V_EXISTE FROM Cliente WHERE cli_id = P_CLI_ID;
        IF V_EXISTE = 0 THEN
          RAISE_APPLICATION_ERROR(-20001, 'No existe registro relacionado en Cliente para cli_id');
        END IF;
      END;
    END IF;

    IF P_EMP_ID IS NOT NULL THEN
      DECLARE V_EXISTE NUMBER;
      BEGIN
        SELECT COUNT(*) INTO V_EXISTE FROM Empleado WHERE emp_id = P_EMP_ID;
        IF V_EXISTE = 0 THEN
          RAISE_APPLICATION_ERROR(-20001, 'No existe registro relacionado en Empleado para emp_id');
        END IF;
      END;
    END IF;

    UPDATE Usuario
       SET username         = P_USERNAME,
           password_hash    = P_PASSWORD_HASH,
           email            = P_EMAIL,
           telefono         = P_TELEFONO,
           rol_id           = P_ROL_ID,
           cli_id           = P_CLI_ID,
           emp_id           = P_EMP_ID,
           ultimo_login_at  = P_ULTIMO_LOGIN_AT,
           bloqueado_hasta  = P_BLOQUEADO_HASTA,
           estado           = P_ESTADO,
           updated_at       = SYSTIMESTAMP
     WHERE usu_id = P_USU_ID AND estado = 'ACTIVO';

    IF SQL%ROWCOUNT = 0 THEN
      RAISE_APPLICATION_ERROR(-20002, 'No se encontró el registro a actualizar en Usuario');
    END IF;
  END SP_ACTUALIZAR;


  PROCEDURE SP_ELIMINAR(
    P_ID IN Usuario.usu_id%TYPE
  ) AS
  BEGIN
    UPDATE Usuario
       SET estado     = 'INACTIVO',
           updated_at = SYSTIMESTAMP
     WHERE usu_id = P_ID
       AND estado = 'ACTIVO';

    IF SQL%ROWCOUNT = 0 THEN
      RAISE_APPLICATION_ERROR(-20003, 'No se encontró el registro a eliminar en Usuario');
    END IF;
  END SP_ELIMINAR;


  PROCEDURE SP_OBTENER(
    P_ID     IN  Usuario.usu_id%TYPE,
    P_CURSOR OUT SYS_REFCURSOR
  ) AS
  BEGIN
    OPEN P_CURSOR FOR
      SELECT usu_id,
             username,
             password_hash,
             email,
             telefono,
             rol_id,
             cli_id,
             emp_id,
             ultimo_login_at,
             bloqueado_hasta,
             created_at,
             updated_at,
             estado
        FROM Usuario
       WHERE usu_id = P_ID AND estado = 'ACTIVO';
  END SP_OBTENER;


  PROCEDURE SP_LISTAR(
    P_CURSOR OUT SYS_REFCURSOR
  ) AS
  BEGIN
    OPEN P_CURSOR FOR
      SELECT usu_id,
             username,
             password_hash,
             email,
             telefono,
             rol_id,
             cli_id,
             emp_id,
             ultimo_login_at,
             bloqueado_hasta,
             created_at,
             updated_at,
             estado
        FROM Usuario
       WHERE estado = 'ACTIVO'
       ORDER BY usu_id DESC;
  END SP_LISTAR;


  PROCEDURE SP_BUSCAR(
    P_VALOR  IN  Usuario.username%TYPE,
    P_CURSOR OUT SYS_REFCURSOR
  ) AS
  BEGIN
    OPEN P_CURSOR FOR
      SELECT usu_id,
             username,
             password_hash,
             email,
             telefono,
             rol_id,
             cli_id,
             emp_id,
             ultimo_login_at,
             bloqueado_hasta,
             created_at,
             updated_at,
             estado
        FROM Usuario
       WHERE UPPER(NVL(TO_CHAR(username), '')) LIKE '%' || UPPER(TO_CHAR(P_VALOR)) || '%'
         AND estado = 'ACTIVO'
       ORDER BY usu_id DESC;
  END SP_BUSCAR;

END PKG_USUARIO;
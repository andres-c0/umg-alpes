CREATE OR REPLACE PACKAGE PKG_LISTA_MATERIALES AS

  PROCEDURE SP_INSERTAR(
    P_PRODUCTO_ID IN Lista_Materiales.producto_id%TYPE,
    P_VERSION IN Lista_Materiales.version%TYPE,
    P_VIGENCIA_INICIO IN Lista_Materiales.vigencia_inicio%TYPE,
    P_VIGENCIA_FIN IN Lista_Materiales.vigencia_fin%TYPE,
    P_ESTADO IN Lista_Materiales.estado%TYPE,
    P_ID OUT Lista_Materiales.lista_materiales_id%TYPE
  );

  PROCEDURE SP_ACTUALIZAR(
    P_LISTA_MATERIALES_ID IN Lista_Materiales.lista_materiales_id%TYPE,
    P_PRODUCTO_ID IN Lista_Materiales.producto_id%TYPE,
    P_VERSION IN Lista_Materiales.version%TYPE,
    P_VIGENCIA_INICIO IN Lista_Materiales.vigencia_inicio%TYPE,
    P_VIGENCIA_FIN IN Lista_Materiales.vigencia_fin%TYPE,
    P_ESTADO IN Lista_Materiales.estado%TYPE
  );

  PROCEDURE SP_ELIMINAR(
    P_ID IN Lista_Materiales.lista_materiales_id%TYPE
  );

  PROCEDURE SP_OBTENER(
    P_ID     IN Lista_Materiales.lista_materiales_id%TYPE,
    P_CURSOR OUT SYS_REFCURSOR
  );

  PROCEDURE SP_LISTAR(
    P_CURSOR OUT SYS_REFCURSOR
  );

  PROCEDURE SP_BUSCAR(
    P_VALOR  IN Lista_Materiales.version%TYPE,
    P_CURSOR OUT SYS_REFCURSOR
  );

END PKG_LISTA_MATERIALES;
/
CREATE OR REPLACE PACKAGE BODY PKG_LISTA_MATERIALES AS

  PROCEDURE SP_INSERTAR(
    P_PRODUCTO_ID IN Lista_Materiales.producto_id%TYPE,
    P_VERSION IN Lista_Materiales.version%TYPE,
    P_VIGENCIA_INICIO IN Lista_Materiales.vigencia_inicio%TYPE,
    P_VIGENCIA_FIN IN Lista_Materiales.vigencia_fin%TYPE,
    P_ESTADO IN Lista_Materiales.estado%TYPE,
    P_ID OUT Lista_Materiales.lista_materiales_id%TYPE
  ) AS
  BEGIN
    DECLARE V_EXISTE NUMBER;
    BEGIN
      SELECT COUNT(*) INTO V_EXISTE FROM Producto WHERE producto_id = P_PRODUCTO_ID;
      IF V_EXISTE = 0 THEN
        RAISE_APPLICATION_ERROR(-20001, 'No existe registro relacionado en Producto para producto_id');
      END IF;
    END;
    INSERT INTO Lista_Materiales (
      producto_id,
      version,
      vigencia_inicio,
      vigencia_fin,
      estado,
      created_at
    ) VALUES (
      P_PRODUCTO_ID,
      P_VERSION,
      P_VIGENCIA_INICIO,
      P_VIGENCIA_FIN,
      NVL(P_ESTADO, 'ACTIVO'),
      SYSTIMESTAMP
    )
    RETURNING lista_materiales_id INTO P_ID;
  END SP_INSERTAR;

  PROCEDURE SP_ACTUALIZAR(
    P_LISTA_MATERIALES_ID IN Lista_Materiales.lista_materiales_id%TYPE,
    P_PRODUCTO_ID IN Lista_Materiales.producto_id%TYPE,
    P_VERSION IN Lista_Materiales.version%TYPE,
    P_VIGENCIA_INICIO IN Lista_Materiales.vigencia_inicio%TYPE,
    P_VIGENCIA_FIN IN Lista_Materiales.vigencia_fin%TYPE,
    P_ESTADO IN Lista_Materiales.estado%TYPE
  ) AS
  BEGIN
    DECLARE V_EXISTE NUMBER;
    BEGIN
      SELECT COUNT(*) INTO V_EXISTE FROM Producto WHERE producto_id = P_PRODUCTO_ID;
      IF V_EXISTE = 0 THEN
        RAISE_APPLICATION_ERROR(-20001, 'No existe registro relacionado en Producto para producto_id');
      END IF;
    END;
    UPDATE Lista_Materiales
       SET producto_id = P_PRODUCTO_ID,
         version = P_VERSION,
         vigencia_inicio = P_VIGENCIA_INICIO,
         vigencia_fin = P_VIGENCIA_FIN,
         estado = P_ESTADO,
         updated_at = SYSTIMESTAMP
     WHERE lista_materiales_id = P_LISTA_MATERIALES_ID AND estado = 'ACTIVO';

    IF SQL%ROWCOUNT = 0 THEN
      RAISE_APPLICATION_ERROR(-20002, 'No se encontró el registro a actualizar en Lista_Materiales');
    END IF;
  END SP_ACTUALIZAR;

  PROCEDURE SP_ELIMINAR(
    P_ID IN Lista_Materiales.lista_materiales_id%TYPE
  ) AS
  BEGIN
    UPDATE Lista_Materiales
       SET estado = 'INACTIVO',
           updated_at = SYSTIMESTAMP
     WHERE lista_materiales_id = P_ID
       AND estado = 'ACTIVO';

    IF SQL%ROWCOUNT = 0 THEN
      RAISE_APPLICATION_ERROR(-20003, 'No se encontró el registro a eliminar en Lista_Materiales');
    END IF;
  END SP_ELIMINAR;

  PROCEDURE SP_OBTENER(
    P_ID     IN Lista_Materiales.lista_materiales_id%TYPE,
    P_CURSOR OUT SYS_REFCURSOR
  ) AS
  BEGIN
    OPEN P_CURSOR FOR
      SELECT lista_materiales_id,
             producto_id,
             version,
             vigencia_inicio,
             vigencia_fin,
             created_at,
             updated_at,
             estado
        FROM Lista_Materiales
       WHERE lista_materiales_id = P_ID AND estado = 'ACTIVO';
  END SP_OBTENER;

  PROCEDURE SP_LISTAR(
    P_CURSOR OUT SYS_REFCURSOR
  ) AS
  BEGIN
    OPEN P_CURSOR FOR
      SELECT lista_materiales_id,
             producto_id,
             version,
             vigencia_inicio,
             vigencia_fin,
             created_at,
             updated_at,
             estado
        FROM Lista_Materiales
       WHERE estado = 'ACTIVO'
       ORDER BY lista_materiales_id DESC;
  END SP_LISTAR;

  PROCEDURE SP_BUSCAR(
    P_VALOR  IN Lista_Materiales.version%TYPE,
    P_CURSOR OUT SYS_REFCURSOR
  ) AS
  BEGIN
    OPEN P_CURSOR FOR
      SELECT lista_materiales_id,
             producto_id,
             version,
             vigencia_inicio,
             vigencia_fin,
             created_at,
             updated_at,
             estado
        FROM Lista_Materiales
       WHERE UPPER(NVL(TO_CHAR(version), '')) LIKE '%' || UPPER(TO_CHAR(P_VALOR)) || '%' AND estado = 'ACTIVO'
       ORDER BY lista_materiales_id DESC;
  END SP_BUSCAR;

END PKG_LISTA_MATERIALES;
/

CREATE OR REPLACE PACKAGE PKG_LISTA_MATERIALES_DETALLE AS

  PROCEDURE SP_INSERTAR(
    P_LISTA_MATERIALES_ID IN Lista_Materiales_Detalle.lista_materiales_id%TYPE,
    P_MP_ID IN Lista_Materiales_Detalle.mp_id%TYPE,
    P_CANTIDAD_REQUERIDA IN Lista_Materiales_Detalle.cantidad_requerida%TYPE,
    P_MERMA_PCT IN Lista_Materiales_Detalle.merma_pct%TYPE,
    P_ESTADO IN Lista_Materiales_Detalle.estado%TYPE,
    P_ID OUT Lista_Materiales_Detalle.lista_materiales_det_id%TYPE
  );

  PROCEDURE SP_ACTUALIZAR(
    P_LISTA_MATERIALES_DET_ID IN Lista_Materiales_Detalle.lista_materiales_det_id%TYPE,
    P_LISTA_MATERIALES_ID IN Lista_Materiales_Detalle.lista_materiales_id%TYPE,
    P_MP_ID IN Lista_Materiales_Detalle.mp_id%TYPE,
    P_CANTIDAD_REQUERIDA IN Lista_Materiales_Detalle.cantidad_requerida%TYPE,
    P_MERMA_PCT IN Lista_Materiales_Detalle.merma_pct%TYPE,
    P_ESTADO IN Lista_Materiales_Detalle.estado%TYPE
  );

  PROCEDURE SP_ELIMINAR(
    P_ID IN Lista_Materiales_Detalle.lista_materiales_det_id%TYPE
  );

  PROCEDURE SP_OBTENER(
    P_ID     IN Lista_Materiales_Detalle.lista_materiales_det_id%TYPE,
    P_CURSOR OUT SYS_REFCURSOR
  );

  PROCEDURE SP_LISTAR(
    P_CURSOR OUT SYS_REFCURSOR
  );

  PROCEDURE SP_BUSCAR(
    P_VALOR  IN Lista_Materiales_Detalle.cantidad_requerida%TYPE,
    P_CURSOR OUT SYS_REFCURSOR
  );

END PKG_LISTA_MATERIALES_DETALLE;
/
CREATE OR REPLACE PACKAGE BODY PKG_LISTA_MATERIALES_DETALLE AS

  PROCEDURE SP_INSERTAR(
    P_LISTA_MATERIALES_ID IN Lista_Materiales_Detalle.lista_materiales_id%TYPE,
    P_MP_ID IN Lista_Materiales_Detalle.mp_id%TYPE,
    P_CANTIDAD_REQUERIDA IN Lista_Materiales_Detalle.cantidad_requerida%TYPE,
    P_MERMA_PCT IN Lista_Materiales_Detalle.merma_pct%TYPE,
    P_ESTADO IN Lista_Materiales_Detalle.estado%TYPE,
    P_ID OUT Lista_Materiales_Detalle.lista_materiales_det_id%TYPE
  ) AS
  BEGIN
    DECLARE V_EXISTE NUMBER;
    BEGIN
      SELECT COUNT(*) INTO V_EXISTE FROM Lista_Materiales WHERE lista_materiales_id = P_LISTA_MATERIALES_ID;
      IF V_EXISTE = 0 THEN
        RAISE_APPLICATION_ERROR(-20001, 'No existe registro relacionado en Lista_Materiales para lista_materiales_id');
      END IF;
    END;
    DECLARE V_EXISTE NUMBER;
    BEGIN
      SELECT COUNT(*) INTO V_EXISTE FROM Materia_Prima WHERE mp_id = P_MP_ID;
      IF V_EXISTE = 0 THEN
        RAISE_APPLICATION_ERROR(-20001, 'No existe registro relacionado en Materia_Prima para mp_id');
      END IF;
    END;
    INSERT INTO Lista_Materiales_Detalle (
      lista_materiales_id,
      mp_id,
      cantidad_requerida,
      merma_pct,
      estado,
      created_at
    ) VALUES (
      P_LISTA_MATERIALES_ID,
      P_MP_ID,
      P_CANTIDAD_REQUERIDA,
      P_MERMA_PCT,
      NVL(P_ESTADO, 'ACTIVO'),
      SYSTIMESTAMP
    )
    RETURNING lista_materiales_det_id INTO P_ID;
  END SP_INSERTAR;

  PROCEDURE SP_ACTUALIZAR(
    P_LISTA_MATERIALES_DET_ID IN Lista_Materiales_Detalle.lista_materiales_det_id%TYPE,
    P_LISTA_MATERIALES_ID IN Lista_Materiales_Detalle.lista_materiales_id%TYPE,
    P_MP_ID IN Lista_Materiales_Detalle.mp_id%TYPE,
    P_CANTIDAD_REQUERIDA IN Lista_Materiales_Detalle.cantidad_requerida%TYPE,
    P_MERMA_PCT IN Lista_Materiales_Detalle.merma_pct%TYPE,
    P_ESTADO IN Lista_Materiales_Detalle.estado%TYPE
  ) AS
  BEGIN
    DECLARE V_EXISTE NUMBER;
    BEGIN
      SELECT COUNT(*) INTO V_EXISTE FROM Lista_Materiales WHERE lista_materiales_id = P_LISTA_MATERIALES_ID;
      IF V_EXISTE = 0 THEN
        RAISE_APPLICATION_ERROR(-20001, 'No existe registro relacionado en Lista_Materiales para lista_materiales_id');
      END IF;
    END;
    DECLARE V_EXISTE NUMBER;
    BEGIN
      SELECT COUNT(*) INTO V_EXISTE FROM Materia_Prima WHERE mp_id = P_MP_ID;
      IF V_EXISTE = 0 THEN
        RAISE_APPLICATION_ERROR(-20001, 'No existe registro relacionado en Materia_Prima para mp_id');
      END IF;
    END;
    UPDATE Lista_Materiales_Detalle
       SET lista_materiales_id = P_LISTA_MATERIALES_ID,
         mp_id = P_MP_ID,
         cantidad_requerida = P_CANTIDAD_REQUERIDA,
         merma_pct = P_MERMA_PCT,
         estado = P_ESTADO,
         updated_at = SYSTIMESTAMP
     WHERE lista_materiales_det_id = P_LISTA_MATERIALES_DET_ID AND estado = 'ACTIVO';

    IF SQL%ROWCOUNT = 0 THEN
      RAISE_APPLICATION_ERROR(-20002, 'No se encontró el registro a actualizar en Lista_Materiales_Detalle');
    END IF;
  END SP_ACTUALIZAR;

  PROCEDURE SP_ELIMINAR(
    P_ID IN Lista_Materiales_Detalle.lista_materiales_det_id%TYPE
  ) AS
  BEGIN
    UPDATE Lista_Materiales_Detalle
       SET estado = 'INACTIVO',
           updated_at = SYSTIMESTAMP
     WHERE lista_materiales_det_id = P_ID
       AND estado = 'ACTIVO';

    IF SQL%ROWCOUNT = 0 THEN
      RAISE_APPLICATION_ERROR(-20003, 'No se encontró el registro a eliminar en Lista_Materiales_Detalle');
    END IF;
  END SP_ELIMINAR;

  PROCEDURE SP_OBTENER(
    P_ID     IN Lista_Materiales_Detalle.lista_materiales_det_id%TYPE,
    P_CURSOR OUT SYS_REFCURSOR
  ) AS
  BEGIN
    OPEN P_CURSOR FOR
      SELECT lista_materiales_det_id,
             lista_materiales_id,
             mp_id,
             cantidad_requerida,
             merma_pct,
             created_at,
             updated_at,
             estado
        FROM Lista_Materiales_Detalle
       WHERE lista_materiales_det_id = P_ID AND estado = 'ACTIVO';
  END SP_OBTENER;

  PROCEDURE SP_LISTAR(
    P_CURSOR OUT SYS_REFCURSOR
  ) AS
  BEGIN
    OPEN P_CURSOR FOR
      SELECT lista_materiales_det_id,
             lista_materiales_id,
             mp_id,
             cantidad_requerida,
             merma_pct,
             created_at,
             updated_at,
             estado
        FROM Lista_Materiales_Detalle
       WHERE estado = 'ACTIVO'
       ORDER BY lista_materiales_det_id DESC;
  END SP_LISTAR;

  PROCEDURE SP_BUSCAR(
    P_VALOR  IN Lista_Materiales_Detalle.cantidad_requerida%TYPE,
    P_CURSOR OUT SYS_REFCURSOR
  ) AS
  BEGIN
    OPEN P_CURSOR FOR
      SELECT lista_materiales_det_id,
             lista_materiales_id,
             mp_id,
             cantidad_requerida,
             merma_pct,
             created_at,
             updated_at,
             estado
        FROM Lista_Materiales_Detalle
       WHERE UPPER(NVL(TO_CHAR(cantidad_requerida), '')) LIKE '%' || UPPER(TO_CHAR(P_VALOR)) || '%' AND estado = 'ACTIVO'
       ORDER BY lista_materiales_det_id DESC;
  END SP_BUSCAR;

END PKG_LISTA_MATERIALES_DETALLE;
/

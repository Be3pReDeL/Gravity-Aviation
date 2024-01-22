using UnityEngine;

public class Bird : Obstacle
{
    [SerializeField] private float horizontalMoveRange = 5f; // Диапазон движения по горизонтали
    [SerializeField] private float horizontalMoveSpeed = 2f; // Скорость движения по горизонтали

    private float originalX;
    private bool movingRight = true; // Изначально птица движется вправо

    private void Start()
    {
        originalX = transform.position.x;
    }

    protected new void Update()
    {
        base.Update(); // Вызов метода Update из базового класса Obstacle

        // Горизонтальное движение
        HorizontalMovement();
    }

    private void HorizontalMovement()
    {
        if (movingRight)
        {
            if (transform.position.x < originalX + horizontalMoveRange)
            {
                transform.Translate(horizontalMoveSpeed * Time.deltaTime, 0, 0);
            }
            else
            {
                movingRight = false;
                FlipSprite();
            }
        }
        else
        {
            if (transform.position.x > originalX - horizontalMoveRange)
            {
                transform.Translate(-horizontalMoveSpeed * Time.deltaTime, 0, 0);
            }
            else
            {
                movingRight = true;
                FlipSprite();
            }
        }
    }

    private void FlipSprite()
    {
        // Отзеркаливание спрайта так, чтобы он смотрел в направлении движения
        var localScale = transform.localScale;
        localScale.x = movingRight ? -Mathf.Abs(localScale.x) : Mathf.Abs(localScale.x);
        transform.localScale = localScale;
    }

}

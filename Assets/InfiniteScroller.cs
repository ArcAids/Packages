using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteScroller
{
    List<RectTransform> transforms;
    Vector2 totalLength;
    float hidingPoint;
    float startingPoint;
    float width;
    public InfiniteScroller(List<RectTransform> transforms)
    {
        this.transforms = transforms;
        foreach (var item in this.transforms)
        {
            totalLength.x+=item.rect.width;
            totalLength.y+=item.rect.height;
        }
        startingPoint = -(totalLength.x * (1 - (1f / transforms.Count)))/2;
        hidingPoint = -startingPoint;
        width = transforms[0].rect.width;
        Debug.Log(hidingPoint +":"+ width);
    }

    public void Scroll(float point)
    {
        float startPosition = (point * totalLength.x)-width/2 ;
        if (startPosition > hidingPoint)
            startPosition = startingPoint -(width-(startPosition-hidingPoint));
        Vector3 objectPosition=transforms[0].localPosition;

        foreach (var item in transforms)
        {
            objectPosition.x = startPosition +width/2;
            item.localPosition = objectPosition;
            startPosition += width;
            if(startPosition > hidingPoint)
                startPosition = startingPoint -(width - (startPosition - hidingPoint));
        }
    }
}


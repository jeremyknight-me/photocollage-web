﻿window.collage = {
    addPhoto: function (id, url, index, settings) {
        let wrapper = document.getElementById('collage-wrapper');
        let frame = document.createElement('div');
        frame.id = `photo-${id}`;
        frame.classList.add('photo-frame');
        frame.classList.add('invisible');
        if (settings.hasBorder) {
            frame.classList.add('bordered');
        }

        frame.style.zIndex = `${index}`;
        const tenPercent = settings.maximumSize * 0.1;
        const boundedHeight = window.innerHeight + tenPercent - settings.maximumSize;
        const boundedWidth = window.innerWidth + tenPercent - settings.maximumSize;
        const positionTop = this.getRandomIntegerInclusive(-tenPercent, boundedHeight);
        const positionLeft = this.getRandomIntegerInclusive(-tenPercent, boundedWidth);
        frame.style.left = `${positionLeft}px`;
        frame.style.top = `${positionTop}px`;
        const rotation = this.getRandomIntFromAbsoluteValue(settings.maximumRotation);
        frame.style.transform = `rotate(${rotation}deg)`;

        let photo = document.createElement('img');
        photo.onload = () => {
            frame.classList.remove('invisible')
        };
        photo.src = url;
        photo.style.maxHeight = `${settings.maximumSize}px`;
        photo.style.maxWidth = `${settings.maximumSize}px`;
        if (settings.isGrayscale) {
            photo.style.filter = 'grayscale(1)';
        }

        let loading = document.getElementById('loading');
        if (loading) {
            loading.remove();
        }

        frame.addEventListener('webkitAnimationEnd', this.handleRemovePhotoAnimationEnd);
        frame.addEventListener('animationend', this.handleRemovePhotoAnimationEnd);

        frame.appendChild(photo);
        wrapper.appendChild(frame);
    },
    removePhoto: function (id) {
        const elementId = 'photo-' + id;
        let element = document.getElementById(elementId);
        element.classList.add("removed");
    },
    getRandomIntFromAbsoluteValue: function (abs) {
        const min = -1 * abs;
        const max = abs
        return Math.floor(Math.random() * (max - min + 1)) + min;
    },
    getRandomIntFromZeroToMax: function (max) {
        const maximum = max + 1;
        return Math.floor(Math.random() * maximum);
    },
    getRandomIntegerInclusive: function (min, max) {
        return Math.floor(Math.random() * (max - min + 1)) + min;
    },
    handleRemovePhotoAnimationEnd: function (e) {
        e.target.remove();
    }
}

class Star {
    constructor() {
        this.x = Math.random() * window.innerWidth;
        this.y = Math.random() * window.innerHeight;
        this.z = Math.random() * window.innerWidth;
        this.radius = 0.5;
    }

    update() {
        this.z -= 6;
        if (this.z <= 0) {
            this.z = window.innerWidth;
            this.x = Math.random() * window.innerWidth;
            this.y = Math.random() * window.innerHeight;
        }
    }

    draw(ctx) {
        const size = (this.radius * window.innerWidth) / this.z;
        const x = (this.x - window.innerWidth / 2) * (size / this.radius);
        const y = (this.y - window.innerHeight / 2) * (size / this.radius);

        ctx.beginPath();
        ctx.arc(x + window.innerWidth / 2, y + window.innerHeight / 2, size, 0, Math.PI * 2);
        ctx.fillStyle = 'white';
        ctx.fill();
    }
}

const canvas = document.createElement('canvas');
canvas.width = window.innerWidth;
canvas.height = window.innerHeight;
canvas.style.position = 'absolute';
canvas.style.top = 0;
canvas.style.left = 0;
canvas.style.zIndex = -1;
document.body.appendChild(canvas);
const ctx = canvas.getContext('2d');
const stars = new Array(150).fill(null).map(() => new Star()); // Reduce the number of stars

let lastTime = 0;
const frameRate = 1000 / 30; // Limit the frame rate to 30 FPS

function animate(time) {
    if (time - lastTime >= frameRate) {
        ctx.fillStyle = 'rgba(0, 0, 0, 0.1)';
        ctx.fillRect(0, 0, canvas.width, canvas.height);
        stars.forEach((star) => {
            star.update();
            star.draw(ctx);
        });
        lastTime = time;
    }
    requestAnimationFrame(animate);
}

animate(lastTime);

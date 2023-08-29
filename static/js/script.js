const slides = document.querySelectorAll('.slide');
const nextButtons = document.querySelectorAll('.next-slide');
const prevButtons = document.querySelectorAll('.prev-slide');
const submitButton = document.querySelector('.submit-button');

let currentSlide = 0;

function updateButtons() {
  nextButtons.forEach(button => {
    button.disabled = currentSlide === slides.length - 1;
  });

  prevButtons.forEach(button => {
    button.disabled = currentSlide === 0;
  });
}

submitButton.addEventListener('click', event => {
  event.preventDefault(); // Prevent default form submission behavior

  const formData = new FormData(document.querySelector('form'));

  fetch('/predict', {
    method: 'POST',
    body: formData,
  })
  .then(response => response.text())
  .then(prediction => {
    const outputTextarea = document.getElementById('output');
    outputTextarea.value = prediction;

    // Move to the next slide after getting the prediction
    if (currentSlide < slides.length - 1) {
      slides[currentSlide].classList.remove('active');
      currentSlide++;
      slides[currentSlide].classList.add('active');
      updateButtons();
    }
  })
  .catch(error => {
    console.error('Error:', error);
  });
});

nextButtons.forEach(button => {
  button.addEventListener('click', () => {
    if (currentSlide < slides.length - 1) {
      slides[currentSlide].classList.remove('active');
      currentSlide++;
      slides[currentSlide].classList.add('active');
      updateButtons();
    }
  });
});

prevButtons.forEach(button => {
  button.addEventListener('click', () => {
    if (currentSlide > 0) {
      slides[currentSlide].classList.remove('active');
      currentSlide--;
      slides[currentSlide].classList.add('active');
      updateButtons();
    }
  });
});

updateButtons();

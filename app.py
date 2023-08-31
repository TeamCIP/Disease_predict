from flask import Flask,request,render_template,url_for
import numpy as np
import pickle

app = Flask(__name__)

with open('flask finally/123.pkl', 'rb') as file:
    loaded_model = pickle.load(file)

@app.route('/')
def hello_world():
    return render_template("23.html")

@app.route('/predict',methods=['POST','GET'])
def predict():
    features=['itching', 'skin_rash', 'continuous_sneezing', 'chills', 'joint_pain',
       'stomach_pain', 'acidity', 'ulcers_on_tongue', 'vomiting',
       'burning_micturition', 'spotting_urination', 'fatigue', 'weight_loss',
       'restlessness', 'lethargy', 'high_fever', 'breathlessness', 'sweating',
       'indigestion', 'headache', 'nausea', 'constipation', 'diarrhoea',
       'mild_fever', 'fluid_overload', 'swelled_lymph_nodes', 'malaise',
       'chest_pain', 'weakness_in_limbs', 'neck_pain', 'dizziness', 'obesity',
       'muscle_weakness', 'stiff_neck', 'movement_stiffness',
       'loss_of_balance', 'muscle_pain', 'red_spots_over_body',
       'family_history', 'mucoid_sputum', 'lack_of_concentration',
       'visual_disturbances', 'blood_in_sputum']
    pred = ['(vertigo) Paroymsal  Positional Vertigo', 'AIDS', 'Acne',
        'Alcoholic hepatitis', 'Allergy', 'Arthritis', 'Bronchial Asthma',
        'Cervical spondylosis', 'Chicken pox', 'Chronic cholestasis', 'Common Cold',
        'Dengue', 'Diabetes ', 'Dimorphic hemmorhoids(piles)', 'Drug Reaction',
        'Fungal infection', 'GERD', 'Gastroenteritis', 'Heart attack', 'Hepatitis B',
        'Hepatitis C', 'Hepatitis D', 'Hepatitis E', 'Hypertension ',
        'Hyperthyroidism', 'Hypoglycemia', 'Hypothyroidism', 'Impetigo', 'Jaundice',
        'Malaria', 'Migraine', 'Osteoarthristis', 'Paralysis (brain hemorrhage)',
        'Peptic ulcer disease', 'Pneumonia', 'Psoriasis', 'Tuberculosis', 'Typhoid',
        'Urinary tract infection', 'Varicose veins', 'hepatitis A']
    selected_features = request.form.getlist('options[]')
    input_values = np.zeros((1, len(features)))
    for feature_index in [features.index(feature) for feature in selected_features]:
        input_values[0, feature_index] = 1

    input_values_int = input_values.astype(int)
    input_values_flat = input_values_int.flatten()
    input_values_2d = input_values_flat.reshape(1, -1)  # Reshape to a 2D array with one row

    predictions = loaded_model.predict(input_values_2d)
    print(input_values_2d)
    
    predicted_disease = pred[predictions[0]]  # Assuming 'predictions' is a single prediction
    print(predicted_disease)
    return render_template('23.html', pred=predicted_disease)
    

if __name__ == '__main__':
    app.run(debug=True)
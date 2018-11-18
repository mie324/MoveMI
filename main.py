'''
    The entry into your code. This file should include a training function and an evaluation function.
'''

import argparse
import numpy;
import matplotlib.pyplot as mpl;
import pandas;
from sklearn.preprocessing import LabelEncoder, OneHotEncoder;
from sklearn.model_selection import train_test_split;

import torch;
from torch.autograd import Variable
from torch.utils.data import DataLoader

from dataset import signalDataset;
from model import ConvolutionalNeuralNetwork;

import os

seed = 0;
torch.manual_seed(0);
numpy.random.seed(0);


trainAcc = [];
valAcc = [];
epochTrack = [];

###
filename = '' + '.npy';

def recieveData():
    #raw = numpy.loadtxt("LabelledData.csv", delimiter=",");
    raw = numpy.array([])

    for data_csv in os.listdir("./data"):
        if data_csv.endswith(".csv"):
            if len(raw) == 0:
                raw = numpy.loadtxt("./data/" + data_csv, delimiter=",")
            else:
                raw = numpy.vstack((raw, numpy.genfromtxt("./data/" + data_csv, delimiter=",")))

    rawL = raw[:,-1]
    rawV = numpy.delete(raw, -1, axis=1)

    return rawV, rawL;

def modifyData(rawV, rawL):
    '''
    do somehthing to modify/normalise the data

    le = LabelEncoder();
    oneHotLabels = le.fit_transform(labels);

    ohe = OneHotEncoder();
    oneHotLabels = ohe.fit_transform(oneHotLabels.reshape(-1,1)).toarray();
    Ndata = normalized.transpose((0,2,1));
    '''
    #for i in range(0, rawV.shape[1]):
    #    mean = numpy.sum(rawV[:, i])
    #    mean /= rawV.shape[1]
    #    std = 0
    #    for j in range(0, rawV.shape[0]):
    #        std += (rawV[j, i] - mean) ** 2
    #    std / rawV.shape[1]
    #    std = std ** (1/2)
    #    for j in range(0, rawV.shape[0]):
    #        rawV[j, i] = (rawV[j, i] - mean) / std

    rawV = numpy.expand_dims(rawV, axis=0) # Temporary to create correct shape
    rawV = rawV.transpose((1, 2, 0))

    label_encoder = LabelEncoder()
    onehot_encoder = OneHotEncoder()
    labels = label_encoder.fit_transform(rawL)
    labels = labels.reshape(-1,1)
    labels = onehot_encoder.fit_transform(labels).toarray()

    modV = rawV
    modL = labels

    return modV, modL

def loadSplitData(modV, modL, batch_size):
# Split the data then load

    TrainV, ValidV, TrainL, ValidL = train_test_split(modV, modL, test_size=0.2, random_state=seed);

    Tset = signalDataset(TrainV, TrainL);
    Tload = DataLoader(Tset, batch_size=batch_size, shuffle=True);

    Vset = signalDataset(ValidV, ValidL);
    Vload = DataLoader(Vset, batch_size=batch_size, shuffle=True);

    return Tload, Vload;

def loadData(modV, modL):
# Load data without splitting
    length = modL.shape
    TestSet = signalDataset(modV, modL);
    TestLoad = DataLoader(TestSet, batch_size=length[0], shuffle=False);

    return TestLoad;

def load_model(lr, input_size, output_size):
    model = ConvolutionalNeuralNetwork(input_size, output_size);
    loss_fnc = torch.nn.MSELoss();
    optimizer = torch.optim.Adam(model.parameters(), lr= lr);

    return model, loss_fnc, optimizer

def evaluate(model, Vload):
    total_corr = 0

    for index, vBatchData in enumerate(Vload):
        vfeature, vlabel = vBatchData;

        outs = model(vfeature.float());

        corr = (torch.argmax(outs.squeeze(), dim=1) == torch.argmax(vlabel, dim=1))

        total_corr += int(corr.sum());

    return float(total_corr)/len(Vload.dataset)

#######################################################################################################################
def main():
    parser = argparse.ArgumentParser()
    parser.add_argument('--batch_size', type=int, default=64)
    #parser.add_argument('--lr', type=float, default= 0.1)
    parser.add_argument('--lr', type=float, default= 0.001)
    parser.add_argument('--epochs', type=int, default= 10)
    parser.add_argument('--eval_every', type=int, default=30)
    parser.add_argument('--training_mode', type=bool, default=True, help='True = training | False = inference')

    args = parser.parse_args()

    rawV, rawL = recieveData()
    modV, modL = modifyData(rawV, rawL);


    if(args.training_mode):
        t = 0

        model, loss_func, optimizer = load_model(args.lr, modV.shape[1], modL.shape[1])

        curr_max_valid_accuracy = 0
        curr_max_index = 0

        for epoch in range(args.epochs):
            accumulated_loss = 0
            total_correct = 0

            train_loader, val_loader = loadSplitData(modV, modL, args.batch_size)

            for i, batch in enumerate(train_loader):
                instances, label = batch
                optimizer.zero_grad()

                predictions = model(instances.float())

                batch_loss = loss_func(input=predictions.squeeze(), target=label.float())
                accumulated_loss += batch_loss
                batch_loss.backward()

                optimizer.step()

                num_correct = torch.argmax(predictions.squeeze(), dim=1) == torch.argmax(label, dim=1)
                total_correct += int(num_correct.sum())


                if(t % args.eval_every == 0):
                    train_accuracy = evaluate(model, train_loader)
                    valid_accuracy = evaluate(model, val_loader)

                    print("Epoch: {}, Step {} | Loss: {} | Number correct: {} | Train accuracy: {} | Validation accuracy: {}".format(epoch + 1, t + 1, accumulated_loss / 100, total_correct, train_accuracy, valid_accuracy))

                    if(valid_accuracy > curr_max_valid_accuracy):
                        curr_max_valid_accuracy = valid_accuracy
                        curr_max_index = t

                    accumulated_loss = 0

                t += 1

        print("Max Validation Accuracy: {} | Occurs at: {}".format(curr_max_valid_accuracy, curr_max_index))
        torch.save(model, "model.pt")

    else:
        try:
            modelName = 'model.pt';
            model = torch.load(modelName);
        except IOError:
            print('Model file {} does not exist'.format(modelName));
            exit(2);


        predictions = [];

        testload = loadData(modV, modL);

        for index, testCase in enumerate(testload):

            temp = model(testCase.float(), testCase.size(0));
            predictions = torch.argmax(temp.squeeze(), dim=1);

        numpy.savetxt('predictions.txt', predictions)


if __name__ == "__main__":
    main()
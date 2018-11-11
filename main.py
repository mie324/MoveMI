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

seed = 0;
torch.manual_seed(0);
numpy.random.seed(0);


trainAcc = [];
valAcc = [];
epochTrack = [];

###
filename = '' + '.npy';

def recieveData():
    raw = numpy.load(filename);
    rawL = raw[:,-1];
    rawV = numpy.delete(raw, -1, axis=1);

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
    pass
    #return modV, modL

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

def load_model(lr):
    model = ConvolutionalNeuralNetwork();
    loss_fnc = torch.nn.MSELoss();
    optimizer = torch.optim.Adam(model.parameters(), lr= lr);

    return model, loss_fnc, optimizer

def evaluate(model, Vload):
    total_corr = 0

    for index, vBatchData in enumerate(Vload):
        vfeature, vlabel = vBatchData;

        outs = model(vfeature.float(), vfeature.size(0));

        corr = (torch.argmax(outs.squeeze(), dim=1) == torch.argmax(vlabel, dim=1))

        total_corr += int(corr.sum());

    return float(total_corr)/len(Vload.dataset)

#######################################################################################################################
def main():
    parser = argparse.ArgumentParser()
    parser.add_argument('--batch_size', type=int, default=64)
    parser.add_argument('--lr', type=float, default= 0.0001)
    parser.add_argument('--epochs', type=int, default=100)
    parser.add_argument('--eval_every', type=int, default=1)
    parser.add_argument('--training_mode', type=bool, default=True, help='True = training | False = inference')

    args = parser.parse_args()

    modV, modL = modifyData(recieveData());


    if(args.training_mode):
        print('load');

        Tload, Vload = loadSplitData(modV, modL, args.batch_size);
        model, loss_func, optimizer = load_model(args.lr);

        runningLoss = 0.0;
        correctPredictions = 0;
        corrPredictsPerStep = 0;
        totalEpochs = 0;
        evalFrequency = 1;

        totalCasesRun = 0;

        bestValAcc = 0;
        bestEpoch = 0;

        print('start');

        for epochCount in range(args.epochs):
            for index, batchData in enumerate(Tload):
                ins, labs = batchData;

                optimizer.zero_grad();

                outs = model.forward(ins.float(), ins.size(0));
                outs = outs.squeeze();

                loss = loss_func(outs, labs.float());
                runningLoss += loss;

                loss.backward();
                optimizer.step();

                corr = (torch.argmax(outs.squeeze(), dim=1) == torch.argmax(labs, dim=1))
                correctPredictions += int(corr.sum());
                corrPredictsPerStep += int(corr.sum());

                totalCasesRun += corr.size(0);

            if (totalEpochs + 1) % evalFrequency == 0:
                epochTrack.append(totalEpochs+1);

                v_accuracy = evaluate(model, Vload);

                valAcc.append(v_accuracy);

                t_accuracy = corrPredictsPerStep / (totalCasesRun);
                corrPredictsPerStep = 0;
                totalCasesRun = 0;

                trainAcc.append(t_accuracy);

                print("Epoch Number: {} , Step {} | Loss: {} | Validation Accuracy: {} | Training Accuracy: {}".format(epochCount+1, totalEpochs+1, runningLoss/100, v_accuracy, t_accuracy));

                runningLoss = 0;

                if(v_accuracy > bestValAcc):
                    torch.save(model, 'model.pt');
                    bestEpoch = totalEpochs+1;
                    bestValAcc = v_accuracy;

            totalEpochs += 1;


        print("Total Training Accuracy: {}".format( float(correctPredictions)/(len(Tload.dataset) * args.epochs)));
        print("Best Validation Accuracy represented in model at epoch {}".format(bestEpoch));

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
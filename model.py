import torch.nn as nn

class ConvolutionalNeuralNetwork(nn.Module):
    def __init__(self):
        super(ConvolutionalNeuralNetwork, self).__init__();

        self.conv1 = nn.Conv1d(20, 16, kernel_size=15, stride=1, padding=9);

        self.conv2 = nn.Conv1d(16, 32, kernel_size=15, stride=1, padding=9);

        self.conv3 = nn.Conv1d(32, 48, kernel_size=15, stride=1, padding=9);

        self.conv4 = nn.Conv1d(48, 64, kernel_size=15, stride=1, padding=9);

        self.conv5 = nn.Conv1d(64, 80, kernel_size=15, stride=1, padding=9);

        self.conv6 = nn.Conv1d(80, 96, kernel_size=15, stride=1, padding=9);

        self.line1 = nn.Linear(96 * 124, 40);

        self.line2 = nn.Linear(40, 26);

    def forward(self, input, batchsize):
        tanh = nn.Tanh();
        relu = nn.ReLU();
        sigmoid = nn.Sigmoid();
        softMax = nn.Softmax(dim= 2);

        #print(input.size());
        x = self.conv1(input);
        x = relu(x);
        #print(x.size());

        x = self.conv2(x);
        x = relu(x);
        #print(x.size());

        x = self.conv3(x);
        x = relu(x);
        #print(x.size());

        x = self.conv4(x);
        x = relu(x);
        # print(x.size());

        x = self.conv5(x);
        x = relu(x);
        # print(x.size());

        x = self.conv6(x);
        x = relu(x);
        # print(x.size());

        x = x.view(batchsize, -1, 96*124);
        #print(x.size());

        x = self.line1(x);
        x = tanh(x);

        x = self.line2(x)

        return x;

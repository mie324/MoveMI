import numpy as np;

class Queue():
    def __init__(self, queueWidth, queueLength):
        self.size = queueLength;
        self.queue = np.zeros((queueLength, queueWidth), dtype=int);
        self.last = 0;

        self.mean = np.zeros(queueWidth, dtype=int);

    def update(self, input):
        self.last = (self.last + 1)% self.size;
        self.queue[self.last] = input;
        self.mean = np.mean(self.queue, 0, int);
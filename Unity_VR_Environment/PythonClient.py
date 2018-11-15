import socket
def sending():
    curr_socket = socket.socket()
    port = 1000
    curr_socket.bind(('127.0.0.1', port))
    curr_socket.listen(1)
    
    i = 0
    
    while True:
        c, address = curr_socket.accept()
        c.sendall(str(i).encode("utf-8"))
        c.close()
        
        i += 1

sending()